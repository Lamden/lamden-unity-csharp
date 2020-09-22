# Official Lamden Unity Plugin
A Unity package to simplify the integration of [Lamden Blockchain](https://lamden.io/) into your Unity project. The plugin also contains an example scene to demonstrate how the package can be used.

# Supported Platforms:
* Standalone (PC)
* Android

## Key Features
* Customize the network information
* Ping the server
* Wallet
* Create new wallet
* Load from SK
* Execute a transaction (smart contract)
* Retrieve Currency Balance
* Retrieve a Contact Variable
* Retrieve Stamp Ratio
* Check maximum stamps available
* Query contact information
  * Get Methods
  * Get contact code

## Installation

1. Download the package `Lamden-Unity.unitypackage` from [HERE](https://github.com/Lamden/lamden-Csharp/releases)
1. In your Unity project select `Assets > Import Package > Custom Package`
1. Select the downloaded package `Lamden-Unity.unitypackage`
1. Ensure all items are selected and choose `Import`

## Usage

Follow the installation steps above to install the `Lamden-Unity` asset package.

The `LamdenManager` from 'Assets/LamdenUnity/Prefabs' is required in your scene to interact with the Lamden blockchain.  The attached `MasterNodeApi` script handles all the calls to the network. 

The `MasterNodeApi` script can be configured with the follow variables:
* Timeout:  Timeout time in milliseconds before the network request is aborted if not answered by the server, Default value: 10000 ms, Type: int
* Network Info
 * Hosts: An array of strings that define the URL of nodes on the lamden network that will be called to execute requests, Default value: http://167.172.126.5:18080/ (Lamden Testnet), Type: string
 * Network Type: Type of Lamden network, Default value: testnet, Type: string
 * Currency symbol: Name of the currency used in the network, Default Value: dTAU, Type: string
 * Lamden: Defines if the network is a Lamden network or not, Default Value: true, Type: boolean
 * Block Explorer: URL to the block explorer for the defined network, Default value: https://testnet.lamden.io/

If you are performing testing with a locally hosted [Lamden mockchain](https://github.com/Lamden/mockchain) with the default configuration change the hosts array to only contain one host of `http://127.0.0.1:8000`.  The other settings should not matter for testing.

Afer adding the `LamdenManager` to the scene the next thing to do is create a Lamden wallet.  This is done using the 'LamdenUnity\Core\Wallet' script.  A wallet is composed of a VK (verification key or public key) and a SK (signing key or private key) keypair.  ***NOTE: Anyone that has copy of a wallet's SK can perform any action on behalf of the owner, so the SK should stored in a secure place.*** For testing purposes on a local mockchain or on the testnet it probably ok to store the SK in plain text or in code. ***NOTE: Keypairs are shared across the lamden testnet and mainnet, ensure that you are not using a keypair that has any TAU or other assets associated with it in your development environment to avoid issues with having the SK post in source or accidently executing a transaction on the mainnet.***

# Creating or Loading Wallet and Accessing Keys
* `New()`: Creates a new keypair (VK and SK)
* `Load(string sk)`: Regenerates keypair (VK and SK) from `sk` argument, in other words passing in the SK will regenerate the VK so both do not need to be stored
* `GetVK()`: Get hex string of the VK (verification key or public key) 
* `GetSK()`: Get hex string of the SK (signing key or private key)

# API Calls
Once a wallet has been generated or loaded API calls can be made through the `MasterNodeApi` is added to the `LamdenManager` gameobject. As the network calls are performed using cooroutines the `MasterNodeApi` must be called attached to a gameobject. The basic structure on all the API calls is to execute an `Action` after the request has been executed and return the results in the action arguments.  The first argument in the `Action` will be a `bool` that indicates if the request was successful.  The other arguments generally contain more infomation and are specific to the action.

There are several useful API calls built into the `MasterNodeApi` script:

## Ping
* **Method:** `PingServer(Action<bool, string> callBack)`
* **Purpose:** Calls the `<master node>/ping` API to determine if the network is up
 * **Action Argurments:**
   * `bool`:The first bool arg will be true if the server responds `online`, if the server is unreachable or reports offline it will respond with false
   * `string`: Returns the error message or json response from the server in as a string

## Get Currency Balance
* **Method:** `GetCurrencyBalance(string key, Action<bool, float> callBack)`: 
* **Purpose:** Retreives the account balance of for the wallet from the server
 * **Argurments:**
   * `key`: The VK of the wallet for the request
 * **Action Argurments:**
   * `bool`: `true` = call successful, `false` = call failed
   * `float`: Successful: The balance of the wallet for a  request, Failed: `-1`

## Get Stamp Ratio
* **Method:** `GetStampRatio(Action<bool, int> callBack)`
* **Purpose:** Get the number of stamps per 1 TAU (stamps are the fee that the sender of the transaction pay for it to be processed)
 * **Action Argurments:** 
   * `bool`: `true` = call successful, `false` = call failed
   * `int`: Successful: The number of stamps per 1 TAU, failed: `-1`

## Get Max Stamps
* **Method:** `GetMaxStamps(string key, Action<bool, int> callBack)`
* **Purpose:** Get the maximum number of stamps a user could spend (stamp ratio * currency balance)
 * **Argurments:**
  * `key`: The VK of the wallet for the request
 * **Action Argurments:**
   * `bool`: `true` = call successful, `false` = call failed
   * `int`: Successful: The maximum stamps the user could spend, failed: `-1`
