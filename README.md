# EncryptLib
Easy encryption library

- Written in C# for an old-colleague of mine requiring an encryption implementation for his game server framework.
- Designed for AES-256.
- Nothing too fancy, nothing too shabby.

```C#
using EncryptLib.Security;

private void ExampleMethod () {
    SetAppConfig();
    string data = "example-data";
    string encrypted = AES256Encrypt(data);
    string decrypted = AES256Decrypt(encrypted);
    Console.WriteLine(decryped); // result: "example-data"
    // Or whatever you want to do with it.
}
```
