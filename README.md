# Gno-Unity-SDK
We realized that Gno is in a stage of active development, showing a lot of potential and anticipating many changes in the future. Hence, we believe that it is necessary to design an SDK that can accommodate this flexibility and the changes that will occur when Gno goes live on the mainnet.

Our solution is to utilize the Tendermint2 library, which is part of Gno. We will create a wrapper to export APIs from Go to the C library using Cgo. This will enable us to run Tendermint2 on various platforms, including Windows, Linux, and MacOS, with the C library.

From this communication, we will develop features such as account management, keys, transactions, and more on .NET by calling C functions from the wrapped library.

In Unity, we will create user interfaces for wallet and NFT features. These will serve as easy-to-understand samples for developers on how to use our SDK.

This strategy not only ensures the scalability of our solution in line with Gno's development but also provides efficient and versatile tools that developers can easily integrate into their projects. We look forward to continuing our research and development in order to provide the best possible SDK for Gno.
![Blank diagram](https://github.com/gnolang/hackerspace/assets/133180467/44449b9d-003d-4fb0-8265-164443fb00fe)
