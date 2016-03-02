# AplayTest
Proof of concept creating a business app in WPF with game network framework "Aplay" from http://www.aplaypowered.com/

Simple project selection dialog is shown at the beginning.

- Searching for projects is done on server-side.
- Checking "CanJoinProject" and "CanCreateProject" is calculated on server side.
- List of available projects is updated by deltas only.
- Service provides projects (i.e. from DB), projects get transformed to Aplay projects.
- Using Rx (https://msdn.microsoft.com/en-us/data/gg577609.aspx), ReactiveProperty (https://reactiveproperty.codeplex.com/), DynamicData (https://github.com/RolandPheasant/DynamicData) to get a nice "push" behavior of data.
- Using Caliburn.Micro for WPF MVVM stuff.


# Getting started
- Download Zip or fork AplayTest
- Start "AplayTest.Server.Console"
- Start one or more instances of "AplayTest.Client.Wpf"
- Search or create a new project. Check other clinet for updates. 
- Join a project (nothing interesting will happen...)

Network adresses are hard-coded in server:

```
 public static void Main()
        {
            var server = new APlayServer(63422, new ProjectManagerFactory(new ProjectManagerService()));
        }
```

and in client (ShellViewModel.cs):

```
 APlayClient.Start("127.0.0.1:63422");
 ```
 
 


