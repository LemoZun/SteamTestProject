using UnityEditor;

public static class BuildAutomator
{
    [MenuItem("Build/Window Build")]
    public static void Build()
    {
        /*BuildPipeline.BuildPlayer(
            scenes, // List of scenes to include in the build
            "E:\\Builds\\GameBuild.exe", // Output path for the build
            BuildTarget.StandaloneWindows, // Target platform for the build
            BuildOptions.None // Build Options
        );*/

        var options = new BuildPlayerOptions
        {
            locationPathName = "D:\\Builds\\GameBuild.exe",
            scenes = new[] { "Assets\\Scenes\\SampleScene.unity" },
            target = BuildTarget.StandaloneWindows,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer((options));

    }
    
    
    public static void BuildAll()
    {
        // Define the build target and options
        BuildTarget target = BuildTarget.StandaloneWindows;
        BuildOptions options = BuildOptions.None;

        // Specify the output path for the build
        string outputPath = "Builds/StandaloneWindows/MyGame.exe";

        // Perform the build
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath, target, options);
        
        // Log a message indicating the build is complete
        UnityEngine.Debug.Log("Build completed successfully!");
    }
}
