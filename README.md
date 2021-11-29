# DresUnityInterface

An interface for [DRES](https://github.com/dres-dev/DRES), meant to be used in [Unity](https://unity.com/).

## Usage / Installation

This is a upm package, add it through the [Unity Package Manager](https://docs.unity3d.com/Manual/Packages.html) as a package from git using the URL `https://github.com/dres-dev/UnityClient.git#main` or directly by adding the following to your `manifest.json` for latest version:

```json
"dev.dres.unityclient": "https://github.com/dres-dev/UnityClient.git#main"
```

## Configuration

This package requires a config file, `dresapi.json` to be located either at

* `Assets/` (corresponds to [`Application.dataPath`](https://docs.unity3d.com/ScriptReference/Application-dataPath.html) for Unity Editor)
* The persistent data folder for your platform (i.e. where [`Application.persistentDataPath`](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html) points to)

The configuration file contains vital information about DRES:

```json
"host": "example.com/dres",
"port": 443,
"tls": true,
"user": "username",
"password": "password"
```

* `host`: the host address of the DRES instance, either an IP or name (defaults to `localhost`)
* `port`: the port of the DRES instance (defaults to `8080`)
* `tls`: whether to use tls or not, i.e. the API calls will be `https` with `true`, `false` otherwise (defaults to `false`)
* `user`: the DRES user name (+)
* `password`: the DRES user passwor (+)

(+): Could be omitted and provided in a separate file (`credentials.json`), as a sibling to the config file:

```json
"user":"username",
"password":"password"
```

## Development

For development, clone this repository into a directory `dev.dres.unityclient` inside the Packages directory of the Unity project you want to develop in.

```
$> git clone git@github.com:dres-dev/UnityClient.git dev.dres.unityclient
```

And subsequently (re-) generate the client libraries:

```
$> gradlew clean deploy
```

For developers, it might be necessary to re-open the unity project multiple times, until the pre-checks are okay.

## Contributors

- Florian Spiess [@Spiess](https://github.com/Spiess)
- Loris Sauter [@sauterl](https://github.com/sauterl)
