# Unity RPG Course - Section 3 - Modifiers And Abilities

Customise character abilities, weapons, characters and enemies. This includes multiple damage types, modifiers, sounds, animations. By the end you can create your core combat experience. The full course is [on Udemy here](https://www.udemy.com/unityrpg).

You're welcome to download, fork or do whatever else legal with all the files!

## How To Build / Compile
This is a Unity project. If you're familiar with source control, then "clone this repo". Otherwise download the contents, and navigate to `Assets > Levels` then open any `.unity` file.

This branch is the course branch, each commit corresponds to a lecture in the course. The current state is our latest progress.

## Lecture List
Here are the lectures of the course for this section...

### 1 Section 3 Introduction

+ What’s coming in this section.


### 2 Design Planning And Decisions

+ Review and update your WBS
+ CHALLENGE
+ Decide on next priorities


### 3 Your Game Design Document

+ GDD Template And Our GDD
+ CHALLENGE


### 4 Unity 5.6 And VS Community 2017
+ My chosen options for upgrading to Unity 5.6
+ Review Unity's release notes


### 5 Using Namespaces In C#
+ What a namespace is and why it's useful
+ How to use namespaces in C#


### 6 The Animator Override Controller
+ Fixing a possible projectile bug
+ What is an Animator Override Controller?
+ How to override animations at runtime


### 7 Protecting Our Interfaces
+ The hidden dependency in our asset pack
+ How to prevent animation events breaking code
+ Challenge: Apply animations to weapons.


### 8 Untangling Standard Assets
+ Taking control of our animations
+ Drawing out our dependencies
+ Taking control of Standard Assets


### 9 Trigger Animations From Code
+ Refactor our Player class
+ Move properties from Player to Weapon


### 10 Triggering Audio On Radius
1. Review AudioTrigger.cs from Gist
2. Set-up and test sound trigger prefab
3. How to source your audio clips


### 11 Terrain Optimization
1. Performance issues from terrain
2. Challenge: Tune terrain settings


### 12 Our First Dialogue
1. Our level's requirements
2. Pitch shifting a character
3. Challenge: create your dialogue


### 13 Tasks And Bugs
1. Our bug fixing workflow
2. Bug tracking and task scheduling
3. Challenge
4. Lets fix some issues


### 14 Placing Props
1. Tidy up scene and import new assets
2. Challenge: Place props in your scene


### 15 Weapon Design
1. Quick look at visual improvements
2. Weapon damage design
3. Prototype requirements for weapons
4. Adding a third weapon and tuning all three


### 16 Changing Your Skybox
1. Lets investigate our scene lighting
2. Import and set up a new skybox


### 17 Bridges And Navmesh
1. Desired bridge look and navmesh issue
2. Create your bridge


### 18 Enemy Mesh Antics
1. Import and set up new character meshes


### 19 Adding An Energy Mechanic
1. Unity 5.6.1 upgrade, Mac differences
2. Extending the player by composition


### 20 Extension By Composition
1. Setup a new delegate in CameraRaycaster
2. De-bounce the right-click button with `GetButtonDown`
3. Subscribe to event in new `Energy.cs` class
4. Reduce energy on each right click
5. Update the energy bar.


### 21 Detecting By Layer In 3D
1. How to use source control to keep refactors honest
2. Our new `CameraRaycaster` architecture


### 22 Simplifying Click To Move
1. Adding `onMouseOverPotentiallyWalkable` event
2. Removing our custom editor script
3. Ensuring click to move still works.
