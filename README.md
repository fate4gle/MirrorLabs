# MirrorLabs

This project aims to establish a generic bridge between a robotic system (operating ROS) and Unity3D using ROS# (Using the 
unity asset [ROS#](https://assetstore.unity.com/packages/tools/physics/ros-107085) published by Siemens). This project is partially derived from the [Rviz2AR](https://github.com/Awesome-Technologies/Rviz2AR) repo published by 
[Awesome-Technologies](https://awesome-technologies.de/). Besides a generic Unity-Ros-Bridge it is planned to modify the [Rviz2AR](https://github.com/Awesome-Technologies/Rviz2AR) project for the recently available HoloLens 2.

The current state of the project includes the following content:

- MirrorLabs_Desktop directory, the complete unity project used directly for development of the generic UR5 ROS-Unity-Bridge. (Executable on Windows 10)
- MirrorLabs_HL2 directory, derivative of the desktop version for Augmented Reality using Microsoft Hololens (0.18362 or higher)
- MirrorLabs_Android directory, derivative of the desktop version for AR/VR using Android based systems 
- Networking directory, here a safe (encrypted) network communication is established, including a sample project (`Mirror-Networking_encrypted MP`) and a necessary Openssl (`Win64OpenSSL-1_0_2u.exe`) executable.

## How to use 
### ROS
Set up ROS (melodic used for development, other may be compatible) environment,  
including the [rosbridge_suit](https://github.com/RobotWebTools/rosbridge_suite),
[ros-sharp ROS-components](https://github.com/siemens/ros-sharp/tree/master/ROS),
[Gazebo](http://gazebosim.org/), and download/install the latest [UR ROS package](https://github.com/ros-industrial/universal_robot).

Once the all packages are installed, open the `ur5.launch` file, located under `universal_robot/ur_gazebo/launch`. 
Add the following lines to the launch procedure: 
```
<include file="$(find file_server)/launch/ros_sharp_communication.launch" > 
     <arg name="port" value="9090"/>
</include>>
 ```

Try out if the installation was succesful by activating the `ur5.launch` file.
```
$ roslaunch ur_gazebo ur5.launch
```

If everything works out, the gazebo GUI will open and the ROS-side of the setup is completed.

Follow this procedure for any robot launch file to enable a connection to Unity-based applications.


### Visual Studio 2019 
Instead of installing Unity first, the visual studio integration seemsto work better 
if Visual Studio is already present during Unity Installations. If Visual Studio 2019 is already installed, feel free to skip step 1,
only open the Visual Studio Installer. Next to the available installation of [Visual Studio 2019](https://visualstudio.microsoft.com/)
click on the `modify` button to enter the extension selection menu

1. Download [Visual Studio 2019](https://visualstudio.microsoft.com/) from [here](https://visualstudio.microsoft.com/downloads/), 
the version does not matter, Visual Studio 2019 Community is already sufficient and free of charge for non-commercial usage. 
(Note: This includes sufficient for HoloLens development!)
This will trigger the Visual Studio Installer to be downloaded, once the download is done, launch the Visual Studio Installer. 
Click through the Visual Studio Setup until it request a specification of extension to be installed.

2. Manage your [Visual Studio 2019](https://visualstudio.microsoft.com/) extension:
Once the overview of extensions is displayed, select the options for `Universal Windows Platform` [UWP, for HoloLens development]. (Make sure `C++ (v142) Universal Windows Platform tools`, 
`C++ (v141) Universal Windows Platform tools` as well as `Windows 10 SDK (10.0.17763.0)` are selected.)
![VS UWP Extension](RM_Graphics/VS_EXT_UWP.PNG)<br/>
![VS UWP Extension Specification](RM_Graphics/VS_EXT_UWP_Spec.PNG)<br/>
Futher, select `Game Development with Unity`, this will enable [Visual Studio 2019](https://visualstudio.microsoft.com/) to directly attach compiled code to [Unity3D](https://unity.com/).
If all extensions are selected hit the `install` (or `modify`, if it an installation was already available) button in the bottom right corner and wait until it is finished.<br/>
![VS Unity Extension](RM_Graphics/VS_EXT_Unity.PNG)<br/>


This concludes the [Visual Studio 2019](https://visualstudio.microsoft.com/) installation. Note: Earlier versions of Visual Studio are not supported for HoloLens developments anymore




### Unity 2019.4.10f1 (or later)


#### Installation
1. Install/Update [UnityHub](https://unity3d.com/get-unity/download), make sure to click on `download Unity Hub`. Once it is downloaded and installed,
click on the left hand side of the UnityHub GUI onto `Installs`. (After a fresh installation of Unity Hub this is empty.) 
Now we add a new installation by clicking on the `Add` button on the top right of the UnityHub GUI. On the appearing pop-up select `Unity 2019.4.3f1` (or later) and click on `Next`.

2. Configure the extensions to be installed alongside the general Unity Editor. Select `Microsoft Visual Studio Community 2019` in the `dev tools`-category, proceed by selecting `Android Build Support` and expanding the options for it. 
There enable `Android SDK & NDK Tools` and `Open JDK` to let Unity get a hold of the latest SDKs, it is possible to do this manually but it is unnecessary extra work. 
Further down make sure that `Universal Windows Platform Build Support` and  `Windows Build Support (IL2CPP)` are selected as well. 
(Within this project, iOS, Mac and Linux platform support is not considered but feel free to costumize your install further).<br\>
![Unity Modules to install](RM_Graphics/Unity_install_modules.PNG)

#### Project Integration

Clone this repo to your local storage (SSD storage highly recommended!), once everything appears in your file explorer, 
open `Unity Hub` and head to `Project` tab (left hand side of the GUI). Here all (to UnityHub) known local projects are displayed, to add the 
`MirrorLabs_XYZ` project, click on the `ADD` button, navigate to the just downloaded repo and select the `MirrorLabs_XYZ` directory.
Now the `Unity Hub` added the new project and by clicking on it, it will launch the Editor. (Note: This can take a moment.)

#### Unity Project Setup

Note: Upon opening the project, a multitude of warnings may be displayed in the console. These can safely be ignored and will be resolved with future updates.

Once the `Unity Editor` is launched, find/locate the `MirrorLabs` folder, open the `scenes` directory and double-click the `ML_UniversalRobotic_ur5` scene. 
This will load the base scene for all UR5 developments, use-case specific scenes will be available within the scene-folder once they are finalized. 
All scenes in development are indicated with the preamble "Dev_", all finalized scenes with "ML_".


#### ML_UniversalRobotic_ur5 scene overview

The current ML_UniversalRobotic_ur5 scene conists of a [UR5](https://www.universal-robots.com/products/ur5-robot/)-object, the `RosConnector`-object, a `Main Camera`-object, a `Directional Light` and an `EventSystem`.
The magic within this Unity project is done using the [ROS#](https://assetstore.unity.com/packages/tools/physics/ros-107085) Unity Asset published by Siemens.  Below follows a quick overview of the purpose of each beforehand mentioned object

1. `UR5 model`, imported using the URDF importer included in the [ROS#](https://assetstore.unity.com/packages/tools/physics/ros-107085) 
asset and modified to be used with the preselected `universal rendering pipeline` [URP]. It serves as direct visualization model of a physical robot.

2. `RosConnector`, contains the networking components which forward the messages, handles  publishing topics, subscribtions etc, between the remote system and the primary ROS environment. This can be configured relatively quickly depending on the purpose. 
Note: Please keep in mind to prevent a loop of messages. (e.g. do not listen to \joint_states while sending these at the same time.)

3. To place the digital robot, the 




![](RM_Graphics/ML_Robot_Mirror.gif)


#### Networking 
The networking project is based on the open source network project `mirror-networking` by [vis2k](https://mirror-networking.com/). To use the networking project, the project `Mirror-Networking_encrypted MP` 
needs to be included as a new unity project (Unity 2019.4.3f1). Further, the project requires a TLS compatible certificate. 
If not present already please follow the youtube tutorial of `Kevin WiBit` which can be found [here](https://www.youtube.com/watch?v=1xtBkukWiek). 
The necessary executable (Windows only) is available in the root folder `Networking`.

![NetworkManager Object in Hierachy](RM_Graphics/NetworkManagerObj.PNG)

With the certificate available, open the `BasicTLSTestScene` within the unity project (Assets/Scenes/BasicTLSTestScene), select the `NetworkManager` Object and change the 
`Certificate Path` of the `Websocket TLS` script to your local certificate (.pfx).

 ![TLS Websocket Component](RM_Graphics/TLSWebsocket.PNG)

Note: Change the password to your certifacte password!


#### Vision and Development

The following developments are currently in progress: 


* Bi-directional ROS communication for `Pose Stamped`-based workflow 
* Streamlining the setup of new Unity scenes by establishing a scene configurator tab within the unity editor UI.
* GUI development for HoloLens 2 (based on the [Rviz2AR](https://github.com/Awesome-Technologies/Rviz2AR) project by [Awesome-Technologies](https://awesome-technologies.de/) )
* GUI development for VR (Oculus Quest).


#### Notes

First start your UR5 simulation within ROS and start the Unity side afterwards, in the `Console Box` of Unity (if still in the editor and not ported to another platform yet), a message regarding the connection state will appear shortly after launching the scene.
(This message will appear and warn of an unsucessful ROS-connection attempt, verify your IP and port parameters and try it again by hitting the `connect` button. Check the `Console Box`-message regarding the outcome.)


