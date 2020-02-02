﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerManager : MonoBehaviour
{
	// This example iterates on the basic multiplayer example by using action sets with
	// bindings to support both joystick and keyboard players. It would be a good idea
	// to understand the basic multiplayer example first before looking a this one.

    public GameObject playerPrefab;
    public Camera cam;
    private float minCamOrthoSize;


    const int maxPlayers = 4;

    List<Vector3> playerPositions = new List<Vector3>() {
            new Vector3( -1, 1, 0 ),
            new Vector3( 1, 1, 0 ),
            new Vector3( -1, -1, 0 ),
            new Vector3( 1, -1, 0 ),
        };



    List<Player> players = new List<Player>(maxPlayers);

    PlayerActions keyboardListener;
    PlayerActions joystickListener;


    void OnEnable()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;
        keyboardListener = PlayerActions.CreateWithKeyboardBindings();
        joystickListener = PlayerActions.CreateWithJoystickBindings();
    }


    void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        joystickListener.Destroy();
        keyboardListener.Destroy();
    }

    private void Awake()
    {
        minCamOrthoSize = cam.orthographicSize;
    }


    void Update()
    {
        if (JoinButtonWasPressedOnListener(joystickListener))
        {
            var inputDevice = InputManager.ActiveDevice;

            if (ThereIsNoPlayerUsingJoystick(inputDevice))
            {
                CreatePlayer(inputDevice);
            }
        }

        if (JoinButtonWasPressedOnListener(keyboardListener))
        {
            //if (ThereIsNoPlayerUsingKeyboard())
            //{
                CreatePlayer(null);
            //}
        }
    }

    void LateUpdate()
    {

        if (players.Count > 0)
        {
            Bounds playersBounds = GetPlayersBoundingBox();
            Vector3 boundCenter = playersBounds.center;

            Matrix4x4 camMatrix = cam.worldToCameraMatrix;
            Vector3 center = camMatrix.MultiplyVector(boundCenter);
            Vector3 max = camMatrix.MultiplyVector(playersBounds.max);
            Vector3 min = camMatrix.MultiplyVector(playersBounds.min);

            float orthoSize;
            if ((playersBounds.size.x / playersBounds.size.y) >= cam.aspect)
            {
                orthoSize = (max.x - min.x) * cam.aspect / 2.0f;
            }
            else
            {
                orthoSize = (max.y - min.y) / 2.0f;
            }

            float padding = orthoSize / 10;
            orthoSize = Mathf.Max(orthoSize + padding, minCamOrthoSize);
            cam.transform.position = new Vector3(center.x, cam.transform.position.y, cam.transform.position.z);
  

            cam.orthographicSize = orthoSize;
        }
    }


    bool JoinButtonWasPressedOnListener(PlayerActions actions)
    {
        return actions.Join.WasPressed;
    }


    Player FindPlayerUsingJoystick(InputDevice inputDevice)
    {
        var playerCount = players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Actions.Device == inputDevice)
            {
                return player;
            }
        }

        return null;
    }


    bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice)
    {
        return FindPlayerUsingJoystick(inputDevice) == null;
    }


    Player FindPlayerUsingKeyboard()
    {
        var playerCount = players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Actions == keyboardListener)
            {
                return player;
            }
        }

        return null;
    }


    bool ThereIsNoPlayerUsingKeyboard()
    {
        return FindPlayerUsingKeyboard() == null;
    }


    void OnDeviceDetached(InputDevice inputDevice)
    {
        var player = FindPlayerUsingJoystick(inputDevice);
        if (player != null)
        {
            RemovePlayer(player);
        }
    }


    Player CreatePlayer(InputDevice inputDevice)
    {
        if (players.Count < maxPlayers)
        {
            // Pop a position off the list. We'll add it back if the player is removed.
            var playerPosition = playerPositions[0];
            playerPositions.RemoveAt(0);

            var gameObject = (GameObject)Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            var player = gameObject.GetComponent<Player>();
            player.PlayerIndex = players.Count;

            if (inputDevice == null)
            {
                // We could create a new instance, but might as well reuse the one we have
                // and it lets us easily find the keyboard player.
                player.Actions = keyboardListener;
            }
            else
            {
                // Create a new instance and specifically set it to listen to the
                // given input device (joystick).
                var actions = PlayerActions.CreateWithJoystickBindings();
                actions.Device = inputDevice;

                player.Actions = actions;
            }

            players.Add(player);

            return player;
        }

        return null;
    }


    void RemovePlayer(Player player)
    {
        playerPositions.Insert(0, player.transform.position);
        players.Remove(player);
        player.Actions = null;
        Destroy(player.gameObject);
    }

    private Bounds GetPlayersBoundingBox()
    {
        Bounds allPlayersBounds = new Bounds(players[0].Center.position, new Vector3());
        players.ForEach((player) =>
        {
            if (player.Center != null)
            {
                allPlayersBounds.Encapsulate(player.Center.position);
            }
        });

        return allPlayersBounds;
    }
}