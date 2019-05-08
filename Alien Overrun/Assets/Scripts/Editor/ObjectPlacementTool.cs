/**
 * Description: Allows mass placement of GameObjects in the editor.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEditor;
using UnityEngine;

// TODO: Add erasing?
// TODO: Add while loop to always try to spawn spawnAmountMax?
// TODO: Add a preview - objects are spawned before clicking, show how they will look once clicked and change constantly while dragging/moving the cursor.

public class ObjectPlacementTool : EditorWindow
{
	private GameObject toPlace = null;

	private GUIStyle toggleButtonStyleOff = null;
	private GUIStyle toggleButtonStylePressed = null;
	private bool isOn = false;
	private bool isOff = true;

	private int spawnAmountMax = 10;
	private float rotationYMin = 0f;
	private float rotationYMax = 360f;
	private float scaleMin = 0.1f;
	private float scaleMax = 3f;
	private float minDistance = 1f;
	private bool scaleMinDistance = true;
	private float brushSize = 10f;
	private Color handleColor = new Color(1f, 1f, 1f, 0.05f);

	private bool useLayer = false;
	private bool useTag = false;
	private LayerMask layerToUse = 0;
	private string tagToUse;

	private readonly GUIContent buttonOnLabel = new GUIContent( "On", "Enables the tool." );
	private readonly GUIContent buttonOffLabel = new GUIContent( "Off", "Disables the tool." );
	private readonly GUIContent objectToPlaceLabel = new GUIContent( "Object", "Object that will be placed." );
	private readonly GUIContent scaleRangeLabel = new GUIContent( "Scale Range", "Min and max scale of the placed objects. Corresponds to the numbers below." );
	private readonly GUIContent scaleMinLabel = new GUIContent( "Min. Scale", "Minimum scale of the placed objects." );
	private readonly GUIContent scaleMaxLabel = new GUIContent( "Max. Scale", "Maximum scale of the placed objects." );
	private readonly GUIContent rotationYRangeLabel = new GUIContent( "Y Rotation Range", "Min and max Y rotation of the placed objects. Corresponds to the numbers below." );
	private readonly GUIContent rotationYMinLabel = new GUIContent( "Min. Y Rotation", "Minimum Y rotation of the placed objects." );
	private readonly GUIContent rotationYMaxLabel = new GUIContent( "Max. Y Rotation", "Maximum Y rotation of the placed objects." );
	private readonly GUIContent scaleDistanceLabel = new GUIContent( "Scale with objects", "Objects that get big scale will keep more space around them and objects that get assigned smaller scale will proportionally get less free space around them." );
	private readonly GUIContent useTagLabel = new GUIContent( "Filter Using a Tag", "Will only show the tool (and by extension allow to place) on objects with a selected Tag." );
	private readonly GUIContent tagToUseLabel = new GUIContent( "Tag", "Will only work on objects that have this Tag." );
	private readonly GUIContent useLayerLabel = new GUIContent( "Filter Using a Layer", "Will only show the tool (and by extension allow to place) on objects with a selected Layer." );
	private readonly GUIContent layerToUseLabel = new GUIContent( "Layer", "Will only work on objects that have this Layer." );
	private readonly GUIContent brushSizeLabel = new GUIContent( "Brush Size", "Radius of the brush." );
	private readonly GUIContent spawnAmountMaxLabel = new GUIContent( "Max. Spawn Amount", "Maximum number of objects placed at once during click or drag. Less objects can be placed if they block each other or the space is tight." );
	private readonly GUIContent handleColorLabel = new GUIContent( "Handle Color", "Color of the handle shown in the Scene View." );

	[MenuItem( "Tools/Object Placement Tool" )]
	static void Init( )
	{
		ObjectPlacementTool window = GetWindow<ObjectPlacementTool>( "Object Placement Tool" );
		window.Show( );
	}

	void OnEnable( )
	{
		SceneView.duringSceneGui += DuringSceneGui;
		Selection.selectionChanged += Repaint;
	}

	void OnDestroy( )
	{
		SceneView.duringSceneGui -= DuringSceneGui;
		Selection.selectionChanged -= Repaint;
	}

	void OnGUI( )
	{
		// Toggle buttons styles
		if ( toggleButtonStyleOff == null )
		{
			toggleButtonStyleOff = "Button";
			toggleButtonStylePressed = new GUIStyle( toggleButtonStyleOff );
			toggleButtonStylePressed.normal.background = toggleButtonStylePressed.active.background;
		}

		// On/Off
		GUILayout.Label( "Enabled", EditorStyles.boldLabel );
		GUILayout.BeginHorizontal( );
		if ( isOn )
			GUI.backgroundColor = Color.green;
		else
			GUI.backgroundColor = Color.white;
		if ( GUILayout.Button( buttonOnLabel, isOn ? toggleButtonStylePressed : toggleButtonStyleOff ) )
		{
			if ( !toPlace )
				Debug.LogError( "<b>Object</b> can't be empty." );
			else if ( Selection.transforms.Length != 1 )
				Debug.LogError( "Please select 1 object in the hierarchy that will serve as a container." );
			else
			{
				isOn = true;
				isOff = false;
			}
		}
		if ( isOff )
			GUI.backgroundColor = Color.red;
		else
			GUI.backgroundColor = Color.white;
		if ( GUILayout.Button( buttonOffLabel, isOff ? toggleButtonStylePressed : toggleButtonStyleOff ) )
		{
			isOn = false;
			isOff = true;
		}
		GUI.backgroundColor = Color.white;
		GUILayout.EndHorizontal( );

		// Selection
		EditorGUILayout.Space( );
		if ( isOn && Selection.transforms.Length != 1 )
		{
			EditorStyles.boldLabel.normal.textColor = Color.red;
			GUILayout.Label( "Please select a container object in the hierarchy!", EditorStyles.boldLabel );
			EditorStyles.boldLabel.normal.textColor = Color.black;
		}

		// Object
		EditorGUILayout.Space( );
		if ( !toPlace )
			GUI.backgroundColor = Color.red;
		GUILayout.Label( "Object to place", EditorStyles.boldLabel );
		toPlace = (GameObject)EditorGUILayout.ObjectField( objectToPlaceLabel, toPlace, typeof( GameObject ), false );
		GUI.backgroundColor = Color.white;

		// Scale
		EditorGUILayout.Space( );
		EditorGUILayout.LabelField( "Spawned objects scale", EditorStyles.boldLabel );
		EditorGUILayout.MinMaxSlider( scaleRangeLabel, ref scaleMin, ref scaleMax, 0.1f, 30f );
		scaleMin = EditorGUILayout.FloatField( scaleMinLabel, scaleMin );
		scaleMax = EditorGUILayout.FloatField( scaleMaxLabel, scaleMax );

		// Rotation
		EditorGUILayout.Space( );
		EditorGUILayout.LabelField( "Spawned objects Y rotation", EditorStyles.boldLabel );
		EditorGUILayout.MinMaxSlider( rotationYRangeLabel, ref rotationYMin, ref rotationYMax, 0f, 360f );
		rotationYMin = EditorGUILayout.FloatField( rotationYMinLabel, rotationYMin );
		rotationYMax = EditorGUILayout.FloatField( rotationYMaxLabel, rotationYMax );

		// Distance
		EditorGUILayout.Space( );
		EditorGUILayout.LabelField( "Min. distance between objects", EditorStyles.boldLabel );
		minDistance = EditorGUILayout.Slider( minDistance, 0f, 100f );
		scaleMinDistance = EditorGUILayout.Toggle( scaleDistanceLabel, scaleMinDistance );

		// Filtering
		EditorGUILayout.Space( );
		EditorGUILayout.LabelField( "Filtering", EditorStyles.boldLabel );

		// Tag to use
		EditorGUILayout.BeginVertical( );
		useTag = EditorGUILayout.Toggle( useTagLabel, useTag );
		if ( useTag )
		{
			if ( tagToUse == "" )
				GUI.backgroundColor = Color.red;
			tagToUse = EditorGUILayout.TagField( tagToUseLabel, tagToUse );
			GUI.backgroundColor = Color.white;
			EditorGUILayout.Space( );
		}
		EditorGUILayout.EndVertical( );

		// Layer to use
		EditorGUILayout.BeginVertical( );
		useLayer = EditorGUILayout.Toggle( useLayerLabel, useLayer );
		if ( useLayer )
			layerToUse = EditorGUILayout.LayerField( layerToUseLabel, layerToUse );
		EditorGUILayout.EndVertical( );

		// Brush options
		EditorGUILayout.Space( );
		GUILayout.Label( "Brush options", EditorStyles.boldLabel );
		brushSize = EditorGUILayout.Slider( brushSizeLabel, brushSize, 0, 100 );
		spawnAmountMax = EditorGUILayout.IntSlider( spawnAmountMaxLabel, spawnAmountMax, 0, 100 );
		handleColor = EditorGUILayout.ColorField( handleColorLabel, handleColor );
	}

	private void DuringSceneGui( SceneView obj )
	{
		if ( isOff || !toPlace || Selection.transforms.Length != 1 )
			return;

		Ray ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );
		if ( Physics.Raycast( ray, out RaycastHit hit ) )
		{
			if ( useTag && tagToUse != "" && !hit.transform.gameObject.CompareTag( tagToUse ) )
				return;

			if ( useLayer && ( layerToUse != hit.transform.gameObject.layer ) )
				return;

			Handles.color = handleColor;
			Handles.DrawSolidDisc( hit.point, hit.normal, brushSize * 0.05f );
			Handles.DrawSolidDisc( hit.point, hit.normal, brushSize );
			Handles.DrawSolidDisc( hit.point, hit.normal, brushSize + scaleMax );

			if ( Event.current.button == 0 && ( Event.current.rawType == EventType.MouseDown || Event.current.rawType == EventType.MouseDrag ) )
			{
				Event.current.Use( );
				GUIUtility.hotControl = GUIUtility.GetControlID( FocusType.Passive );

				PlaceMultipleObjects( hit );
			}
		}
	}

	private void PlaceMultipleObjects( RaycastHit originalHit )
	{
		for ( int i = 0; i < spawnAmountMax; i++ )
		{
			Ray rayForObject = new Ray( originalHit.point + originalHit.normal, -originalHit.normal );
			rayForObject.origin += Quaternion.LookRotation( originalHit.normal ) * ( Random.insideUnitCircle * brushSize );

			float newObjectScale = Random.Range( scaleMin, scaleMax );
			float distanceBetwenObjects = minDistance;

			if ( Physics.Raycast( rayForObject, out RaycastHit hit ) )
			{
				// Check distance to other elements
				bool skipRestOfTheLoop = false;
				foreach ( Transform other in Selection.transforms[0] )
				{
					if ( scaleMinDistance )
						distanceBetwenObjects = ( minDistance / 2 * newObjectScale ) + ( minDistance / 2 * other.localScale.x );

					if ( Vector3.Distance( hit.point, other.position ) <= distanceBetwenObjects )
					{
						skipRestOfTheLoop = true;
						break;
					}
				}
				if ( skipRestOfTheLoop )
					continue;

				GameObject newObject = PrefabUtility.InstantiatePrefab( toPlace, Selection.transforms[0] ) as GameObject;
				newObject.transform.position = hit.point;
				newObject.transform.rotation = Quaternion.identity;
				newObject.transform.up = hit.normal;
				newObject.transform.localScale = Vector3.one * newObjectScale;
				newObject.transform.Rotate( Vector3.up * Random.Range( rotationYMin, rotationYMax ), Space.Self );

				Undo.RegisterCreatedObjectUndo( newObject, "Placed " + newObject.name + " using Object Placement Tool" );
			}
		}
	}
}
