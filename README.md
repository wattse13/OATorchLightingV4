# OATorchLightingV4
 Fourth Attempt at the Welding Module Template
 Eli Watts
 September 3rd, 2021

 ## Introduction

 When I started this project, I knew very little about code. I still don’t know much about code, but I know a little bit more now. I know enough to know that a lot of decisions I made while creating this prototype probably were not the correct choice. It works, sort of, but needs extensive refactoring before I would say that it works well.

## Broad Strokes

Delegates, delegates, delegates everywhere!
When an interactable object is clicked on, a delegate event sends that `GameObject` to all of its subscribers. These subscribers are then able to use the data stored in the `EquipmentClass` component on each `GameObject` to perform their functions.

## A Brief Overview of the Classes

The `EquipmentClass` Class is responsible for storing each interactable object’s name, description, image, etc. It also initializes each object’s starting values, and it can enable/disable colliders when necessary.
The `OnClickDelegate` Class sends all of its subscribers a reference to the `GameObject` that was clicked on.
The `ClickMenuController` Class controls the menu the player uses to decide between inspecting or activating each piece of equipment.
The `InspectMenuController` Class controls the text and buttons displayed by the Inspect or Use menu. It also tells the `EquipmentController` Class when the player has decided to activate or replace an object.
The `EquipmentController` Class handles events where data in the `EquipmentClass` Class needs to be altered, like when an object is activated. It also moves the object or changes the objects currently displayed image when necessary.
The `SafetyStateController` Class determines when a mistake has been made by the player and what kind of mistake it was. It stores most of the actual game logic and is dire need of refactoring.
The `GameEvents` Class determines which message to send to the `ModalWindow` Class based on what kind of message it receives from the `SafetyStateController`.
The `ModalWindow` Class displays a message in a pop-up window based on what kind of message it receives from the `GameEvents` Class.

## What Works

I tried to build this prototype in a modular manner, and I think I succeeded in many ways. When scripts are coupled, they are coupled using delegates. This makes adding new modules easy as they can be added or taken away without breaking everything else.
The UI exists in its own scene. It is then loaded into the scene with everything else when the game starts. This makes it easy to re-use the UI across different scenes, lessons, or even modules.

## What Doesn’t Work

A lot. Most scripts are not as loosely coupled as I would like. Adding/removing new functionality on top of what already exists is straightforward. However, trying to remove one of the scripts that currently exists would probably be disastrous as many scripts need the data passed through delegate events by other scripts to function correctly.
The `SafetyStateController` Class is a disaster. It is a mess of if-statements and variables. At the moment it works half-way correctly but trying to add more equipment or more possible consequences is a nightmare. This is in part due to the magic of permutations. The current six pieces of equipment offer a total of 720 number of possible order combinations. This is already too many to manage, and it doesn’t consider that each object can be in a total of four states.
A lot of this code is specific to the prototype. This is especially true of the `SafetyStateController` where most of the game logic happens. More generalized systems are necessary if this code is going to be reused in other lessons or modules.
Despite how contained each Class may have sounded in my brief overview, the truth is that they are not. Many Classes repeat functionality that other classes do. There are no clear protocols for where data is accessed, how data is passed, or who passes data. As much as I wanted a hub-like structure, with `EquipmentClass` and `EquipmentController` at the center, things don’t work quite as intended.
It is ugly. Things are not arranged nicely. There are no animations. There are no sounds.

## The Ugly Details

### `EquipmentClass Class`

The equipment class defines and holds data related to each interactable object in the scene. These properties include the object’s:
* Name
* Description Variants
* Image Variants
* Position
* Safe Boolean
* Active Boolean
This class uses the `SetInitialValues()` function to set each object’s starting data. The functions `DisableColliders()` and `EnableColliders()` are triggered by delegate events sent by either the `ClickMenuController` or the `InspectMenuController`. When either Controller class opens, or closes, a menu it invokes a delegate event which tell the `EquipmentClass` to disable all colliders. This is done to prevent clicking on other interactable objects in the scene while a menu is open.
My initial hope was that this class and the `EquipmentController` class would work as a sort of hub and that other classes would request the data they needed from these two classes. However, many functions in other classes bypass this hub function by using the `TryGetComponent( out )` function to retrieve data from the clicked on object directly.

### `OnClickDelegate Class`

This class invokes a delegate event every time an object with a collider is clicked on. The delegate event, `OnClicked`, sends a reference to the clicked on `GameObject` to all the scripts subscribed to the event. It needs a reference to the Main Camera in the UI scene to work.

### `ClickMenuController Class`
This class controls the menu which opens after an object has been clicked on. It displays the object name and gives the player the option to either inspect or activate the equipment.
This class is subscribed to the `OnClickDelegate` Class’ `OnClicked` delegate event. When this event is invoked, it triggers two functions in the `ClickMenuController` Class. `ShowClickMenu()` opens the menu and uses the `TryGetComponent( out )` to get the necessary data from the `EquipmentClass` component. It also invokes the `ClickMenuController` delegate event `OnClickMenu` which triggers the enable/disable collider functions in `EquipmentClass`. The MoveClickMenu() insures that the menu does not display off screen. If statements decide which side of the screen the object is on and then adds or subtracts an offset to the position of the click menu transform to place it on either the left or right side of the object.

### `InspectMenuController Class`
This class controls which text and which buttons appear on both the Inspect and Use Menus. It also adds or removes a background blur when either menu is open.
When the `OnClicked` delegate event is registered by the `InspectMenuController`, the function SetCurrentPrefabValues() uses the `TryGetComponent( out )` function to get the clicked-on object’s data values. These values are then assigned to corresponding variables. These variables are then used by the functions SetInspectText() and SetUseText() to modify what the two menus display.
The class is subscribed to delegate events from both the `EquipmentController` Class and the `SafetyStateController` Class. These delegate events update the variables to reflect the object’s safe and active statuses. Both `UpdatePrefabSafetyStatus()` and `UpdatePrefabActiveStatus()` are triggered by delegate events. They use the `TryGetComponent ( out )` to get the object’s updated safe or active status. They then call the `SetUseText()`, `SetInspectText()` or `ChangeUseButton()` to update the displayed text `GameObject`s.

### `EquipmentController Class`
This class should be the only class which can change values in the `EquipmentClass` Class. As such, it is used to change the safe and active Boolean values in `EquipmentClass`, and it changes the object’s position on screen.
Unlike other scripts, this script accesses the currently clicked-on `GameObject` data in two different ways. The first method is through the usual `TryGetComponent( out )` method, and the second method is through the two functions `GetCurrentPrefab()` and `SetCurrentPrefab(`). `SetCurrentPrefab()` takes a `GameObject` as a parameter and then sets the variable `currentPrefab` to that passed in `GameObject`. `GetCurrentPrefab()` simply returns the value of `currentPrefab`. I don’t think that this is particularly efficient, or useful, and it should probably be refactored.
When this script registers an `OnClicked` event, it calls the methods `SetCurrentPrefab()` and `SetCurrentPrefabValues()`. These methods assign the object’s data to corresponding variables. Although this script assigns all of the object’s data to different variables, it only actually ends up using its image, position, safe bool, and active bool. This isn’t intentional design and reflects more that I didn’t really know what I was doing at the time.
The functions `SetSafetyValue()` and `SetActiveValue()` are called respectively through the Replace Button and Activate Button `GameObject`s. Those buttons are found in the `InspectUseMenu` `GameObject`. When called these functions invoke their own delegate events which trigger methods in the `InspectMenuController` to update the displayed text.
The methods in the Focus Prefab Region are called every time the Inspect or Use menu is opened. These functions center the prefab on the screen, move it up onto the focus sorting layer, and disable the object’s collider.
The methods in the Restore Prefab Region are called every time the Inspect or Use menu is closed. They simply move the object back into its original position and sorting layer.

### `SafetyStateController Class`
This script is probably the messiest and most convoluted. It needs extensive refactoring. It uses dictionaries, lists, and hash sets to keep track of each object’s active state, safety state, and the order in which things have been activated. It does all of this so that it can set off events when certain criteria have been fulfilled, i.e., the acetylene regulator was activated before the acetylene cylinder which results in a regulator burnout event.
A regulator burnout event fires when either the regulator is activated before the corresponding cylinder, or a regulator with a status of unsafe is activated after a cylinder is activated. To check for this, the script uses the `Oxy/AcetylRegBurnoutTest()` and `CompareLists()` methods. First, the `Oxy/AcetylRegBurnoutTest()` adds an item to the `Oxy/AcetylRegBurnout` list if it does not already exist in said list. Then the function calls the `CompareLists()` method which compares the list created by the `Oxy/AcetylRegBurnoutTest()` to the `Oxy/AcetylRegBurnoutTrue` list. If the lists are unequal in length, or the lists are not equal to each other, no burnout occurs. If the lists are equal, the `OnConsequence` delegate event is invoked.
A leak event occurs when the torch is activated before a regulator/cylinder is successfully activated, when a hose is in the unsafe state while a regulator/cylinder are both active, or when the torch is active and in the unsafe state while a regulator/cylinder pair are also active. Rather than always checking if both the regulator/cylinder are active, after regulator/cylinder pair are successfully activated a corresponding Boolean value, `isOxyPressure/isAcetylPressure`, is set to true.
The `LeakTest()` method checks if `isOxy/AcetylPressure` is true or not. If there’s no pressure, the system can’t leak. If there is pressure, it then checks where in the `ActiveStatesPlayer` list the torch currently sits. If the torch object is at a lower index value, meaning it was activated before both the regulator and cylinder, then the system is leaking. This invokes an `OnConsequence` delegate event.
The Leak test() method also checks if `isOxy/AcetylPressure` is true while either of the hoses are still in the unsafe state. If true, another `OnConsequence` delegate event is invoked.
An explosion event occurs when the `isAcetylLeak` Boolean value is true and the lighter element in its safe or unsafe state is used. To check for this, the `ExplosionTest()` checks the values of the `isAcetylLeak` and if the lighter has been used in its safe or unsafe variant. If true, a `OnConsequence` delegate event is invoked.
To determine which test to run, the script uses the `ConsequenceCoordinator()` method. This method uses the hash set, `currentActiveEquipment`, to decide which method to call. For example, if the hash set contains integers which correspond to the oxygen cylinder and the oxygen regulator, it calls the `oxy/acetylRegBurnoutTest()` method.
The `OnConsequence` delegate event sends an integer, which corresponds to different events, to its subscriber, the `GameEvents` class. It is currently set up that the variables, which hold the different integers, need to match between the two Classes for the correct event to be fired by the `GameEvents` class. This is a very fragile system and needs to be changed. Really, this entire script needs to be refactored as the crazy if-statement trees are difficult to debug and often do not behave as desired.

### `GameEvents Class`
This class is subscribed to the `SafetyStateController` Class. When it receives a delegate event, it calls a method based on the integer the delegate event sent. The method `WhoCalled()` coordinates which event is called based on the sent integer value.
As stated in the `SafetyStateController` Class description, these event integer variables have to match those in the `SafetyStateController` class. This is a very fragile system which needs to be replaced, for example with a system that uses unique events for each possible consequence. I have not yet done that as I would prefer to refactor the `SafetyStateController` first.

### `ModalWindow Class`
This class is subscribed to the `GameEvents` `ResetMessageSent` and `ContinueMessageSent` delegate events. Depending on the severity of the mistake, the `GameEvents` Class will invoke a delegate event which triggers the either the `ResetMessageRecieved()` or `ContinueMessageRecieved()` methods. These methods cause the modal window to pop-up onto the screen with either a continue button, or a reset button. The modal window also contains a little text-message describing the mistake the player made.
When the player makes a big enough mistake, they must restart. When the player has made only a minor mistake, they are given the option to continue.

### `RestartLevel Class`
This class simply reloads the level. It is triggered when the reset button is clicked after a big mistake.

### `UILoader Class`
This class loads the UI scene into the currently open scene additively. Things are set up this way so that UI elements can be reused across different lessons or modules.

### `Tutorial Class`
This class is basically just a copy of the `ModalWindow` Class, but it also contains a list of text objects which can be edited in the visual editor. When the last message has been displayed it destroys itself.
