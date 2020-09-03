using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionBuilder
{    
    void StartTranscation(TxInfo txInfo)        
    {
        // NOTE: The same host must be used throughout the transaction, specifically the tx needs to be sent to the same
        // host that the nonce was received from

        if(txInfo == null) Debug.LogError("txInfo cannot be null");
        if (Helper.isValidKeyString(txInfo.senderVk)) Debug.LogError("Sender Public Key Required (Type: Hex String");
        if (!string.IsNullOrEmpty(txInfo.contractName)) Debug.LogError("Contract Name Required (Type: String)");
        if (!string.IsNullOrEmpty(txInfo.methodName)) Debug.LogError("Method Required (Type: String)");
    }

    void MakePayLoad()
    {
        // Create JSON string of txInfo and sort it

        /**
              this.payload = {
                contract: this.contract,
                function: this.method,
                kwargs: this.kwargs,
                nonce: this.nonce,
                processor: this.processor,
                sender: this.sender,
                stamps_supplied: this.stampLimit
            }
            this.sortedPayload = this.sortObject(this.payload)
         **/
    }

    void MakeTransaction()
    {
        /**
            this.tx = {
            metadata:
                {
                signature: this.signature,
                    timestamp: parseInt(+new Date / 1000),
                },
                payload: this.sortedPayload.orderedObj
            }
        **/
    }

    void VerifySignature()
    {
        // Verify the signature is correct
        /**
            if (!this.transactionSigned) throw new Error('Transaction has not be been signed. Use the sign(<private key>) method first.')
            const stringBuffer = Buffer.from(this.sortedPayload.json)
            const stringArray = new Uint8Array(stringBuffer)
            return wallet.verify(this.sender, stringArray, this.signature)
        **/
    }

    void Sign(string sk)
    {
        /**
            //const stringBuffer = Buffer.from(this.sortedPayload.json)
            //const stringArray = new Uint8Array(stringBuffer)
            //this.signature = wallet.sign(sk, stringArray)
            //this.transactionSigned = true;
        **/
    }

    void SortObject(object obj)
    {
        /**
            const processObj = (obj) => {
                const getType = (value) => {
                    return Object.prototype.toString.call(value)
                }
                const isArray = (value) => {
                    if (getType(value) === "[object Array]") return true;
                    return false;
                }
                const isObject = (value) => {
                    if (getType(value) === "[object Object]") return true;
                    return false;
                }


                const sortObjKeys = (unsorted) => {
                    const sorted = { };
                    Object.keys(unsorted).sort().forEach(key => sorted[key] = unsorted[key]);
                    return sorted
                }


                const formatKeys = (unformatted) => {
                    Object.keys(unformatted).forEach(key => {
                        if (isArray(unformatted[key])) unformatted[key] = unformatted[key].map(item => {
                            if (isObject(item)) return formatKeys(item)
                            return item
                        })
                        if (isObject(unformatted[key])) unformatted[key] = formatKeys(unformatted[key])
                    })
                    return sortObjKeys(unformatted)
                }


                if (!isObject(obj)) throw new TypeError('Not a valid Object')
                    try
                {
                    obj = JSON.parse(JSON.stringify(obj))
                    }
                catch (e)
                {
                    throw new TypeError('Not a valid JSON Object')
                    }
                return formatKeys(obj)
            }
            const orderedObj = processObj(object)
            return {
                orderedObj, 
                json: JSON.stringify(orderedObj)
            }
        **/
    }

    void GetNonce(Action callback)
    {
        /**
            let timestamp = new Date().toUTCString();
            this.nonceResult = await this.API.getNonce(this.sender)
            if (typeof this.nonceResult.nonce === 'undefined'){
                throw new Error(`Unable to get nonce for ${ this.sender}
                on network ${ this.url}`)
            }
            this.nonceResult.timestamp = timestamp;
            this.nonce = this.nonceResult.nonce;
            this.processor = this.nonceResult.processor;
            //Create payload object
            this.makePayload()

            if (!callback) return this.nonceResult;
            return callback(this.nonceResult)
        **/
    }

    void send(string sk, Action callback)
    {
        /**
                //Error if transaction is not signed and no sk provided to the send method to sign it before sending
                if (!validateTypes.isStringWithValue(sk) && !this.transactionSigned)
                {
                    throw new Error(`Transation Not Signed: Private key needed or call sign(<private key>) first`);
                        }

        let timestamp = new Date().toUTCString();

                        try{
                            //If the nonce isn't set attempt to get it
                            if (isNaN(this.nonce) || !validateTypes.isStringWithValue(this.processor)) await this.getNonce();
                            //if the sk is provided then sign the transaction
                            if (validateTypes.isStringWithValue(sk)) this.sign(sk);
                            //Serialize transaction
                            this.makeTransaction();
        //Send transaction to the masternode
        let response = await this.API.sendTransaction(this.tx)
                            //Set error if txSendResult doesn't exist
                            if (response === 'undefined' || validateTypes.isStringWithValue(response)){
            this.txSendResult.errors = ['TypeError: Failed to fetch']
                            }else{
            if (response.error) this.txSendResult.errors = [response.error]
                                else this.txSendResult = response
                            }
        } catch (e){
                            this.txSendResult.errors = [e.message]
                        }
                        this.txSendResult.timestamp = timestamp
                        return this.handleMasterNodeResponse(this.txSendResult, callback)
        **/
    }

    void checkForTransactionResult(Action callBack)
    {
        /**
                    return new Promise((resolve) => {
                    let timerId = setTimeout(async function checkTx() {
                        this.txCheckAttempts = this.txCheckAttempts + 1;
                        const res = await this.API.checkTransaction(this.txHash)
                            let checkAgain = false;
                        const timestamp = new Date().toUTCString();
                        if (typeof res === 'undefined')
                        {
                            this.txCheckResult.error = 'TypeError: Failed to fetch'
                            }
                        else
                        {
                            if (res.error)
                            {
                                if (res.error === 'Transaction not found.')
                                {
                                    if (this.txCheckAttempts < this.txCheckLimit)
                                    {
                                        checkAgain = true
                                        }
                                    else
                                    {
                                        this.txCheckResult.errors = [res.error, `Retry Attmpts ${ this.txCheckAttempts}
                                        hit while checking for Tx Result.`]
                                        }
            }else{
                                        this.txCheckResult.errors = [res.error]
                                    }
                                }else{
                                    this.txCheckResult = res;
                                }
                            }
                            if (checkAgain) timerId = setTimeout(checkTx.bind(this), 1000);
                            else{
                                if (validateTypes.isNumber(this.txCheckResult.status)){
                if (this.txCheckResult.status > 0)
                {
                    if (!validateTypes.isArray(this.txCheckResult.errors)) this.txCheckResult.errors = []
                                        this.txCheckResult.errors.push('This transaction returned a non-zero status code')
                                    }
            }
                                this.txCheckResult.timestamp = timestamp
                                clearTimeout(timerId);
            resolve(this.handleMasterNodeResponse(this.txCheckResult, callback))
                            }
                        }.bind(this), 1000);
                    })
        ***/
    }

    void HandleMasterNodeResponse(object result, Action callback )
    {
        //Check to see if this is a successful transacation submission
        /**
            
            if (validateTypes.isStringWithValue(result.hash) && validateTypes.isStringWithValue(result.success)){
                this.txHash = result.hash;
                this.setPendingBlockInfo();
            }else{
                this.setBlockResultInfo(result)
                this.txBlockResult = result;
            }
            this.events.emit('response', result, this.resultInfo.subtitle);
            if (validateTypes.isFunction(callback)) callback(result)
            return result
         **/
    }

    void SetPendingBlockInfo()
    {
        /**
              this.resultInfo =  {
                title: 'Transaction Pending',
                subtitle: 'Your transaction was submitted and is is being processed',
                message: `Tx Hash: ${this.txHash}`,
                type: 'success',
            }
            return this.resultInfo;
         * */
    }

    void SetBlockResultInfo(object result)
    {
        /**
          let erroredTx = false;
            let errorText = `returned an error and `
            let statusCode = validateTypes.isNumber(result.status) ? result.status : undefined
            let stamps = (result.stampsUsed || result.stamps_used) || 0;
            let message = '';
            if(validateTypes.isArrayWithValues(result.errors)){
                erroredTx = true;
                message = `This transaction returned ${result.errors.length} errors.`;
                if (result.result){
                    if (result.result.includes('AssertionError')) result.errors.push(result.result)
                }
            }
            if (statusCode && erroredTx) errorText = `returned status code ${statusCode} and `
          
            this.resultInfo = {
                title: `Transaction ${erroredTx ? 'Failed' : 'Successful'}`,
                subtitle: `Your transaction ${erroredTx ? `${errorText} ` : ''}used ${stamps} stamps`,
                message,
                type: `${erroredTx ? 'error' : 'success'}`,
                errorInfo: erroredTx ? result.errors : undefined,
                returnResult: result.result || "",
                stampsUsed: stamps,
                statusCode
            };
            return this.resultInfo;
         * */
    }

    void GetResultInfo()
    {
        /**
            return this.resultInfo;
         * */
    }

    void GetTxInfo()
    {
        /**
            return {
            senderVk: this.sender,
            contractName: this.contract,
            methodName: this.method,
            kwargs: this.kwargs,
            stampLimit: this.stampLimit
             }
         * */
    }

    void getAllInfo()
    {
        /**
            return {
            uid: this.uid,
            txHash: this.txHash,
            signed: this.transactionSigned,
            signature: this.signature,
            networkInfo: this.getNetworkInfo(),
            txInfo: this.getTxInfo(),
            txSendResult: this.txSendResult,
            txBlockResult: this.txBlockResult,
            resultInfo: this.getResultInfo(),
            nonceResult: this.nonceResult
            }
         * */
    }

}