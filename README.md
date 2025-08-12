README – Importing Unity Project and Running Code

📌 Prerequisites
Before you start, ensure that you have the following:

Unity Hub installed (Download here)

Unity Editor installed (matching the version the project was developed in)

A compatible .NET SDK/Mono version (Unity installs these automatically)

Git (optional, if you’re cloning the repository from source)

📂 Step 1 – Get the Project Files
You can obtain the Unity project in one of the following ways:

Download ZIP – Download the project folder as a .zip file and extract it.

Clone Repository – If hosted on GitHub:

bash
Copy
Edit
git clone <repository_url>
⚠️ Important: The Unity project folder must contain an Assets/, Packages/, and ProjectSettings/ directory.

🖥 Step 2 – Open the Project in Unity Hub
Launch Unity Hub.

Click Add (top right corner).

Navigate to the root folder of your Unity project (the folder containing Assets/, Packages/, and ProjectSettings/).

Select the folder and click Open.

If prompted, Unity Hub will suggest installing the correct Unity Editor version for the project — install it for full compatibility.

⚙ Step 3 – Let Unity Import the Project
When the Unity Editor opens the project for the first time, it will import all assets and compile scripts.

This process may take a few minutes, depending on the project size.

▶ Step 4 – Running the Code
In Unity’s Project panel, ensure your scene is loaded.

Go to File > Open Scene and select the .unity file in the Assets/Scenes/ folder.

Set the Game View resolution if needed (top of the Game tab).

Click the Play ▶ button at the top of the Unity Editor to run the project.

Observe the output in:

The Game window for visuals

The Console window for logs and errors (Window > General > Console)

🛠 Step 5 – Modifying & Testing Code
Locate the C# scripts inside the Assets/Scripts/ folder.

Open scripts in Visual Studio or Rider (configured via Edit > Preferences > External Tools).

Save changes and return to Unity — it will automatically recompile scripts.

🧹 Step 6 – Building the Project (Optional)
To create a standalone build:

Go to File > Build Settings.

Select your target platform (PC, Mac, Android, etc.).

Click Add Open Scenes to include the current scene.

Click Build and choose a destination folder.
