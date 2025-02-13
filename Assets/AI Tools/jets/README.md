---
license: cc-by-4.0
library_name: unity-sentis
---

# Jets Text-to-Speech Model validated for Unity Sentis (Version 1.4.0-pre.2*)
*Version 1.3.0 Sentis files are not compatible with version 1.4.0 and above and will need to be recreated

This is a text to speech model called [Jets](https://huggingface.co/imdanboy/jets). It takes in a text string which you convert to phonemes using a dictionary and then outputs a wav to play the voice.

## How to Use
* Create a new scene in Unity 2023
* Install `com.unity.sentis` version `1.4.0-pre.2` package
* Put the c# script on the Main Camera
* Put the `sentis` file and the `phoneme_dict.txt` file in the `Assets/StreamingAssets` folder
* Add an AudioSource component on the Main Camera
* Set the `inputText` string for what you want it to say
* Press play

## Information
This version uses a phoneme dictionary to convert the text into a string of phonemes. There are other ways to do this, for example using another model, or heuristics.

Since we are using a simple dictionary it has no way of distinguishing heteronyms (two words with the same spelling but different pronounciation).

## License
Attribution for the original creators is required. See[Jets](https://huggingface.co/imdanboy/jets) for more details.

You must retain the copyright notice in the `phoneme_dict.txt` file.