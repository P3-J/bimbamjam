using Godot;
using System;
using static Godot.ResourceLoader;

public partial class SceneLoader : ProgressBar
{
	string loadDir;
	Node currentLoadedNode;
	private Tween tween;
	bool isLoading = false;
	public static SceneLoader instance;
	public override void _Ready()
	{
		instance = this;
	}

	public override void _Process(double delta)
	{
		if (!isLoading) return;
		if (loadDir == "") return;

		Godot.Collections.Array loadPercent = new();
		ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(loadDir, loadPercent);
		double percentage = (double)loadPercent[0];

		if (status == ResourceLoader.ThreadLoadStatus.Loaded)
		{
			if (tween.IsRunning()) return;
			isLoading = false;
			tween = GetTree().CreateTween();
			tween.TweenProperty(this, "value", percentage, 0.5);
			tween.TweenCallback(Callable.From(InstantiateScene));
		}

		if (Value == percentage) return;
		if (tween != null) return;

		tween = GetTree().CreateTween();
		tween.TweenProperty(this, "value", percentage, 0.5);
	}

	private void InstantiateScene()
	{
		var resource = ResourceLoader.LoadThreadedGet(loadDir) as PackedScene;
		currentLoadedNode = resource.Instantiate();
		GetTree().Root.AddChild(currentLoadedNode);
		this.Visible = false;
	}
	public void LoadObject(string objectPath)
	{
		if (isLoading) return;

		loadDir = objectPath;
		ResourceLoader.LoadThreadedRequest(loadDir);
		Visible = true;
		Value = 0;
		if (currentLoadedNode == null) return;
		currentLoadedNode.QueueFree();
	}
}
