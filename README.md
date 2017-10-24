# SnailLock

## Add the package to your project:
1) Import the [Unity Package](https://github.com/theepicsnail/SnailLock/raw/master/export.unitypackage)

**Optional (Advanced)** Clone this repository into your Assets directory.

## Add a lock to your world
1) Add an empty GameObject
1) Add Component 'Snail Lock' to that object

## Set your password/passwords
1) Set a Size (This is the number of different passwords you want to use. Probably 1.)
1) Fill out the Element 0...'s with your password(s).

## Secure stuff
1) Click 'Create Lock'
1) "OnUnlocked" is created as a child of your lock.
1) Put anything restricted under that GameObject.

# Chaning passwords:
Change the passwords as you see fit. 

Clicking 'Create Lock' again will make it use the new passwords.

You will need to upload the new world for the new passwords.
