# Xamarin.Forms Mobile project

# Architecture
* Mobile.Android
* Mobile.UWP
* Mobile.iOS
* Mobile
* Mobile.ViewModels
* Mobile.ViewModels.UnitTests
* Mobile.Core
* Mobile.Core.UnitTest

# Notes:
Running an android emulator and trying to target an API that is hosted on localhost isn't as straight forward. Since the localhost in the emulator's case is the the device it self not the host machine.

A work around is to:
* clone https://bitbucket.org/jacqijvv/barelycompetent.sharpproxy.zip/src/master/
* Extract it
* run the exe file
* put the internal port to be the same as the port on which the api is running
* set the external port which we will use
* in API service temporary replace localhost:5000/api with your *ip/externalPort*
* Rebuild the solution
* Run