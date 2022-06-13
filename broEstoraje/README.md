## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
It is used to perform typical operations of a FileSystem, delete, move, copy, rename and compress in zip.  
I highly recommend having [boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) installed to use this component. So you know about the states.

## Usage
**Compress a folder:**  
```sh
/DirToZip <folderPath> <zipFilePath(must include extencion)>  
```
Example:  
```sh
/DirToZip C:\Users\Zhenboro\Desktop C:\Users\Zhenboro\Escroto.zip  
```
---
**Decompress zip to folder:**  
```sh
/ZipToDir <zipPath> <dirPath>  
```
Example:  
```sh
/ZipToDir C:\Users\Zhenboro\Escroto.zip C:\Users\Zhenboro\Desktop  
```
---
**Rename:**  
```sh
/RenameFile <filePath> <newName>  
```
```sh
/RenameDirectory <folderPath> <newName>  
```
Example:  
```sh
/RenameFile C:\Users\Zhenboro\Escroto.zip Escritorio.zip  
```
```sh
/RenameDirectory C:\Users\Zhenboro\Carpeta0 Carpeta1  
```
---
**Copy:**  
```sh
/CopyFile <filePath> <newFilePath>  
```
```sh
/CopyDirectory <folderPath> <newFolderPath>  
```
Example:  
```sh
/CopyFile C:\Users\Zhenboro\Escroto.zip C:\Users\Zhenboro\Copia.zip  
```
```sh
/CopyDirectory C:\Users\Zhenboro\Carpeta0 C:\Users\Zhenboro\Carpeta1  
```
---
**Move:**  
```sh
/MoveFile <filePath> <newFilePath>  
```
```sh
/MoveDirectory <folderPath> <newFolderPath>  
```
Example:  
```sh
/MoveFile C:\Users\Zhenboro\Escritorio.zip C:\Users\Zhenboro\Desktop\Escritorio.zip
```
```sh
/MoveDirectory C:\Users\Zhenboro\Carpeta0 C:\Users\Zhenboro\Desktop\Carpeta0  
```
---
**Upload:**  
Just like the `/Payloads.Upload.File=<filepath>,<null, phpPost>` command. you can upload a file to the server.  
```sh
/Upload <filePath> 
```
Example:  
```sh
/Upload C:\Users\Zhenboro\Desktop\Escritorio.zip 
```
It will be sent to the `fileUpload.php` on the server  

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**