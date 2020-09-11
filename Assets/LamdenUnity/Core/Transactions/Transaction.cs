using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LamdenUnity
{
    public class Transaction
    {
        public enum TransactionStatus { Error, Sending, SubmittedProcessing, Completed  };
        private TxData txData;
        private TxInfo txInfo;
        private MasterNodeApi masterNodeApi;
        private Action<TransactionStatus, TxResponse> onCompleteAction;
        private string txUri;

        public const string replaceString = "\"toReplace\":\"**ReplaceMe**\"";

        private int checkStatusAttempts = 5;

        public Transaction(MasterNodeApi node, TxInfo ti, Action<TransactionStatus, TxResponse> action)
        {
            txInfo = ti;
            masterNodeApi = node;
            onCompleteAction = action;
            if (!isValidaTxInfo())
                return;

            txData = new TxData();            
            txData.payload.sender = txInfo.sender.GetVK(); ;
            txData.payload.contract = txInfo.contractName;
            txData.payload.function = txInfo.methodName;
            txData.payload.stamps_supplied = txInfo.stampLimit;

            GetNonce();
        }

        bool isValidaTxInfo()
        {
            if (masterNodeApi == null)
            {
                TxError("StartTranscation: masterNodeApi cannot be null.");
                return false;
            }
            if (txInfo == null)
            {
                TxError("StartTranscation: txInfo cannot be null.");
                return false;
            }
            if (!Helper.isValidKeyString(txInfo.sender.GetVK()))
            {
                TxError("StartTranscation: Sender Public Key Required (Type: Hex String).");
                return false;
            }
            if (string.IsNullOrEmpty(txInfo.contractName))
            {
                TxError("StartTranscation: Contract Name Required (Type: String).");
                return false;
            }
            if (string.IsNullOrEmpty(txInfo.methodName))
            {
                TxError("StartTranscation: Method Required (Type: String).");
                return false;
            }
            if (txInfo.stampLimit < 0)
            {
                TxError("StartTranscation: Stamp Limt must be to equal or greater than 0.");
                return false;
            }
            if (txInfo.kwargs == null)
            {
                TxError("StartTranscation: kwargs cannot be null");
                return false;
            }

            return true;
        }

        void TxError(string error)
        {
            Debug.LogError(error);
            TxResponse txResponse = new TxResponse();
            txResponse.error = error;
            onCompleteAction(TransactionStatus.Error, txResponse);
        }

        void GetNonce()
        {
            // TODO: Add callback to include URL
            masterNodeApi.GetNonce(txInfo.sender.GetVK(),(bool success, string result, string uri) => {
                if (!success)
                {
                    TxError($"Transaction: Failed to get nonce: {result}");
                }
                else
                {
                    txUri = uri;
                    NonceData nonce = NonceData.FromJson(result);
                    txData.payload.nonce = nonce.nonce;
                    txData.payload.processor = nonce.processor;                  
                    SendTransaction();
                }
            });
        }

        private void SendTransaction()
        {           
            string payloadJson = JsonUtility.ToJson(txData.payload);
            string kwargs = txInfo.getKwargString();
            payloadJson = payloadJson.Replace(replaceString, kwargs);
            
            
            txData.metadata.signature = txInfo.sender.GetSignatureString(payloadJson);

            Debug.Log($"sig: {txData.metadata.signature}\n\npayload: {payloadJson}");

            txData.metadata.timestamp = Helper.GetDateStamp();
            
            string txDataJson = JsonUtility.ToJson(txData).Replace(replaceString, kwargs);            
            Debug.Log($"Sending txData Json data: {txDataJson}");

            masterNodeApi.SendTransaction(txUri, txDataJson, (bool success, string result) =>
            {
                Debug.Log($"SendTransaction result was {success}: {result}");

                TxResponse txResponse = JsonUtility.FromJson<TxResponse>(result);

                if (!success || result.Contains("error"))
                {
                    onCompleteAction(TransactionStatus.Error, txResponse);
                }
                else
                {
                    onCompleteAction(TransactionStatus.SubmittedProcessing, txResponse);
                    CheckStatus(txResponse);
                }
                
            });            
        }

        private void CheckStatus(TxResponse txResponse)
        {            
            Thread.Sleep(500);            
            masterNodeApi.CheckTransaction(txUri, txResponse.hash, (bool success, string json) => { 
                if(success)
                {
                    CheckTransactionData checkTransactionData = JsonUtility.FromJson<CheckTransactionData>(json);
                    var n = JSON.Parse(json);
                    checkTransactionData.state = n["state"];
                    txResponse.transactionData = checkTransactionData;
                    txResponse.transactionData.transaction.payload.kwargs = n["transaction"]["payload"]["kwargs"];
                    if (checkTransactionData.status == 0)
                        onCompleteAction(TransactionStatus.Completed, txResponse);
                    else
                    {
                        txResponse.error = txResponse.transactionData.result;
                        onCompleteAction(TransactionStatus.Error, txResponse);
                    }

                    return;
                }

                if (checkStatusAttempts-- > 0)
                    CheckStatus(txResponse);
                else
                    TxError("Failed to check status of the transaction.");

            });
        }  

    }
}