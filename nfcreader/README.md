# Salausavaimen luominen
Tekee tiedoston *nfcreader.keystore* aliaksella *nfcreader*
```
keytool -genkey -v -keystore nfcreader.keystore -keyalg RSA -keysize 2048 -validity 10000 -alias nfcreader
```

# APK:n luominen
Käytä edellä tehtyä tiedostoa, avainta sekä salasanoja
```
cordova build android --release -- --keystore="nfcreader.keystore" --storePassword=nfcreader --password=nfcreader --alias=nfcreader
```