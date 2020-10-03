﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Plugins.Core.PathCore;

public class TrackPathManager : MonoBehaviour
{
    static TrackPathManager _instance;

    public static TrackPathManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<TrackPathManager>();
            }
            return _instance;
        }
    }

    public Train targetTrain;

    private List<GameObject> targets = new List<GameObject>();

    private List<Track> tracks;
    private List<TweenerCore<Vector3, Path, PathOptions>> activePaths;

    private void Awake()
    {
        _instance = this;
        tracks = new List<Track>();
        activePaths = new List<TweenerCore<Vector3, Path, PathOptions>>();
    }

    public void RegisterTrack(Track track)
    {
        if (!tracks.Contains(track))
        {
            tracks.Add(track);
            tracks.Sort((a, b) => (a.transform.position.x.CompareTo(b.transform.position.x)));
        }
    }

    public void DemoAnimation()
    {
        if(!Application.isPlaying) { return; }

        // Step one: build the track waypoints
        List<Vector3> waypoints = new List<Vector3>();
        foreach (Track track in tracks) {
            // Todo: this is crazy fucking slow my dudes lol
            List<Vector3> localWaypoints = new List<Vector3>(track.LocalWaypoints());
            List<Vector3> newWaypoints = new List<Vector3>(localWaypoints.Select(v => v + track.transform.position));
            waypoints = new List<Vector3>(waypoints.Concat(newWaypoints));
        }

        // Step two: find the train cars that need to follow this path
        targets.Clear();
        foreach(TrainCar car in targetTrain.cars)
        {
            targets.Add(car.gameObject);
        }

        const float speed = 0.8f;

        // Step three: set up tweening animations for each train car, offset by the train car's offset from the train car leader
        var leaderPosition = targets[0].transform.position;
        var tailPosition = targets[targets.Count() - 1].transform.position;
        activePaths.Clear();
        for (int i = 0; i < targets.Count(); i++)
        {
            GameObject target = targets[i];
            var offsetFromLeader = (leaderPosition - target.transform.position).magnitude;
            var offsetFromTail = (tailPosition - target.transform.position).magnitude;

            Vector3 cachedStart = Vector3.zero;
            Vector3 cachedEnd = Vector3.zero;

            var startingDirectionVector = (waypoints[1] - waypoints[0]).normalized;
            var endingDirectionVector = (waypoints[waypoints.Count() - 1] - waypoints[waypoints.Count() - 2]).normalized;
            cachedStart = waypoints[0];
            cachedEnd = waypoints[waypoints.Count() - 1];
            waypoints[0] -= startingDirectionVector * offsetFromLeader;
            waypoints[waypoints.Count() - 1] += endingDirectionVector * offsetFromTail;

            target.transform.position = waypoints[0];
            var result = target.transform.DOPath(waypoints.ToArray(), 2, PathType.CatmullRom, PathMode.TopDown2D);

            waypoints[0] = cachedStart;
            waypoints[waypoints.Count() - 1] = cachedEnd;
            
            result.SetLookAt(.01f, true);
            result.SetEase(Ease.InOutQuad);
            result.SetSpeedBased();

            activePaths.Add(result);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    float timeScale = 1.0f;
    void Update()
    {
        if (activePaths.Count() > 0)
        {
            //timeScale -= (0.1f * Time.deltaTime);
            //foreach(var path in activePaths)
            //{
            //    path.timeScale = timeScale;
            //}
        }
    }
}
