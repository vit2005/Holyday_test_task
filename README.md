We were tasked with making a quick prototype of a simple clicker game. The concept is that you click bananas to get coins, and with the coins you get you can upgrade your bananas. Bananas are auto generated every few seconds.

We made the prototype and submitted it to QA. Unfortunately it seems we rushed it more than we should because QA found a lot of issues with it and sent us back a list of issues to fix:


The info button does not work
The coin counter does not work and its graphic seems stretched
The bananas are sometimes spawned outside of viewport (if the screen size changes)
The upgrades are not kept between sessions*
The game’s background is not tiled as it was originally intended

We were also tasked into expanding the prototype by adding the following features:


A popup that will open when clicking our avatar icon. The graphics for it are already added in the project.
Inside it, an option to switch language between English and Russian (Русский) (must be kept between sessions*)
Achievements, which can also be found inside the popup, with milestones** that will work as banana upgrades (must be kept between sessions*). There are 2 achievements that we must implement, total number of bananas clicked and total number of upgrades made.

Here is a mockup of the popup that our artists have provided:
![image](https://github.com/user-attachments/assets/5bec7d16-d256-461f-97d9-306e1cdf1e84)

The popup must be at the same position on the screen as it is on the mockup, and it should have the same close functions as the info popup, plus a semi transparent background, again as the info popup. The gold displayed next to the bananas should be the total increase gained from the specific achievement, as it is on the upgrades (if the user has +30 gold from the achievement, we should display +30, not +1). Finally, the bar should display our current progress and the progress needed to reach the next achievement milestone (on the mockup, 31 is our current progress, 32 is the next milestone, but there can be a lot of milestones after 32, at 40, 50, 60 etc.).

Our back end developer has already finished his relevant tasks and has given us some info:


When the game starts we get this response from the server: SetUserData. Its data length should now be raised to 4. Number 1 is the user’s upgrades and number 2 is their gold as they were before. Number 3 will be the user’s language and number 4 will be their achievements. User’s language is a simple int value (as gold), achievements are an object that is the same as the upgrades object we get from the server (the object contains the user’s achievement progress, not their completed milestones, this should be calculated on the client).
In order to retrieve the new data (3 and 4, language and achievements) you must set the boolean parameter of the GetUserData function we send to the server to true. Besides allowing you to retrieve the new information, it also allows achievements to be taken into account for gold calculations server side.
The developer also added 2 new functions that we can send from the client to the server:
ChangeLanguage, which takes one parameter, the index of the language.
The translations and languages are already added to the project, you can find them on the script Translator.
AddAchievementProgress, which takes two parameters, the achievement index and the extra progress on it. The only achievement we should update is the total number of bananas clicked and we cannot send more than 10 clicks at once. The achievements’ data are also added to the project already, you can find them in the script Achievements.
The achievement of the total number of upgrades is auto calculated server-side when we use the Upgrade function. 

Finally, our supervisor also asked us to do any improvements to the existing code we might see fit. Unfortunately, we are on a tight schedule and we must have everything done by the same time two days from now!

Good luck, have fun 🙂

[*] Kept between sessions means that you should get updated data from the server, not to save data yourself. Nothing on this project should have or be done with locally saved data.

[**]  Milestones are like specific points of the achievements’ progress that will provide extra gold per banana click (for instance, if you reach 100 total banana clicks you will be getting extra gold from bananas from now on).

[***] The project is on Unity version 6000.0. Please use the same one for your submission. You are not required to build the project, your submission should be the Unity project itself.


