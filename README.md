# Gno-Unity-SDK
We realized that Gno is in a stage of active development, showing a lot of potential and anticipating many changes in the future. Hence, we believe that it is necessary to design an SDK that can accommodate this flexibility and the changes that will occur when Gno goes live on the mainnet.

Our solution is to utilize the Tendermint2 library, which is part of Gno. We will create a wrapper to export APIs from Go to the C library using Cgo. This will enable us to run Tendermint2 on various platforms, including Windows, Linux, and MacOS, with the C library.

From this communication, we will develop features such as account management, keys, transactions, and more on .NET by calling C functions from the wrapped library.

In Unity, we will create user interfaces for wallet and NFT features. These will serve as easy-to-understand samples for developers on how to use our SDK.

This strategy not only ensures the scalability of our solution in line with Gno's development but also provides efficient and versatile tools that developers can easily integrate into their projects. We look forward to continuing our research and development in order to provide the best possible SDK for Gno.
![Blank diagram](https://private-user-images.githubusercontent.com/133180467/290098553-44a7a688-199f-4ca9-99f7-1f9ca47300b4.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTEiLCJleHAiOjE3MDI0NTA0MjYsIm5iZiI6MTcwMjQ1MDEyNiwicGF0aCI6Ii8xMzMxODA0NjcvMjkwMDk4NTUzLTQ0YTdhNjg4LTE5OWYtNGNhOS05OWY3LTFmOWNhNDczMDBiNC5wbmc_WC1BbXotQWxnb3JpdGhtPUFXUzQtSE1BQy1TSEEyNTYmWC1BbXotQ3JlZGVudGlhbD1BS0lBSVdOSllBWDRDU1ZFSDUzQSUyRjIwMjMxMjEzJTJGdXMtZWFzdC0xJTJGczMlMkZhd3M0X3JlcXVlc3QmWC1BbXotRGF0ZT0yMDIzMTIxM1QwNjQ4NDZaJlgtQW16LUV4cGlyZXM9MzAwJlgtQW16LVNpZ25hdHVyZT0xMTExMWRhNDM2MTJlMDdmOWU5MTk4ZThiNjhiYjhhMTJjYmU1ZTUxMzkyOGRhOTA1NmVkM2VlZDAyYTJjMDRiJlgtQW16LVNpZ25lZEhlYWRlcnM9aG9zdCZhY3Rvcl9pZD0wJmtleV9pZD0wJnJlcG9faWQ9MCJ9.KF1lCNqXAfIq6MNM47MQ5pXZscUro4ykPUj7jl8t5WE)
