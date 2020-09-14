# Official Lamden Unity Plugin
A Unity package to simplify the integration of [Lamden Blockchain](https://lamden.io/) into your Unity project. The plugin also contains an example scene to demonstrate how the package can be used.

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

1. Download the package from [HERE](/releases/latest)
1.  In your Unity project select 'Assets > Import Package > Custom Package'
1. Select the downloaded package
1. Ensure all items are selected and choose 'Import'

## How to Use

1. Add the "LamdenManager" from 'Assets/LamdenUnity/Prefabs' to your scene
1. * Optional: Select the "LamdenManager" from the 'Hierarchy' and in the 'Inspector' edit the network info under the 'Master Node Api' script (note: it is prepopulated with the Lamden testnet information)
1. See examples under 'Assets/Lamden UI Example'
  1. 'LamdenTest' for: Ping, Wallet Creation and Wallet Balance
  1. 'Test_Values' for submitting a transaction
1. See the Test Cases under 'Assets/LamdenUnity/EditorTests' for examples of other usage
