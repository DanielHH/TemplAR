An updated version of the map (level1-v2) can be found in the branch 'demo-map-fix'.

# Prerequisites
1. Unity
2. AR Foundation 1.0.0 PREVIEW
3. Post Processing 2.1.4
Make sure to install the AR foundation and Post Processing package before
attempting to build!

# How to build this game in Android
1. Clone this repo.
2. Open up the project (TDDD57-Labs) in Unity.
3. Select 'File' > 'Build Settings...'
4. Select the scene 'Scenes/Level1-v2'
5. Choose Android as the Platform.
6. Plug in you android phone into the computer. 
7. Select your device as 'Run Device'.
8. Press 'Build And Run'
9. Name and create an apk-file. 
10. Press 'Save' and wait for it to build on your phone.

# How to play
This game is primarily designed to be played with one hand with the smartphone 
held in portraitmode.

When the game starts, point the camera toward a horizontal plane, like a floor 
or a table with distinguishable features, and move it around until a transparent 
plane and an indicator starts to take form.

## Place the map
The yellow indicator, which is always in the middle of your screen, indicates 
where the map will be placed. When you're ready to place the map tap, with two 
fingers, anywhere against the screen. 

This is the only time you may need to use two hands. Promise!

## Pick up an object
An object which the player can pick up, so called 'pickupables', will get green 
when the indicator is on it. To pick up the object, aim the indicator against it 
and tap one finger anywhere against the screen. 

## Drop an object
To dropt the object, tap one finger anywhere against the screen. 

## Rotate an object
To rotate an object, which is being carried, press one finger anywhere against 
the screen and slide it either to the right or the left. 
The object will then start rotating in that direction as long as the finger is 
held like this. To stop rotating, either let go of the screen or slide the 
finger back in the other direction.

## Move an object toward and away from the camera
To move an object toward or away from you, which is being carried, press one 
finger anywhere against the screen and slide it either down or up. 
The object will then start moving in one direction as long as the finger is 
held like this. To stop moving the object, either let go of the screen or slide the 
finger back in the other direction.

## Place an object in the empty spots along the path
To place an object in the empty spots along the path three things are required:
1. It has to be the right type of object. For example, a stairwell can't be placed in a spot where a straight path road is required.
2. The object to place has to be in the spot where it needs to be placed.
3. The object must be rotated correctly (with an error margin of 30 degrees).