using System.Collections;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private Vector2[] _pathMarkers;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _stopDuration = 5;
    private bool _stopped;

    private int _currentPathMarkerIndex;

    private void Update()
    {
        if (_stopped)
        {
            return;
        }

        Vector2 targetPos = _pathMarkers[_currentPathMarkerIndex];
        transform.localScale = targetPos.x >= transform.position.x ? Vector3.one : new(-1, 1, 1);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);

        if ((Vector2)transform.position == targetPos)
        {
            StartCoroutine(NextPathMarker());
        }
    }

    private IEnumerator NextPathMarker()
    {
        _stopped = true;

        yield return new WaitForSeconds(_stopDuration);

        _currentPathMarkerIndex = ++_currentPathMarkerIndex % _pathMarkers.Length;
        _stopped = false;
    }
}
