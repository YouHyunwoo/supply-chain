using System.Collections.Generic;
using UnityEngine;

public class TransportLine
{
    public Node startingNode;
    public Node endingNode;
    public List<Vector2Int> locations;
    public float speed = 0.1f;

    public List<Transport> transports = new ();

    public void Update()
    {
        for (int i = 0; i < transports.Count; i++)
        {
            var transport = transports[i];
            if (transport.cargo == null) {
                transports.RemoveAt(i);
                i--;
                continue;
            }
            transport.progress += speed * Time.deltaTime;
            if (transport.progress >= 1.0f)
            {
                transport.currentInterval++;
                transport.progress = 0.0f;

                if (transport.currentInterval >= locations.Count - 1)
                {
                    transports.RemoveAt(i);
                    i--;
                    transport.cargo.CompleteTransport();
                    if (endingNode == null)
                    {
                        // 운송 끝
                    }
                    else
                    {
                        endingNode.ReceiveCargo(transport.cargo);
                    }
                    continue;
                }
            }

            transport.cargo.transform.position = Vector3.Lerp(
                new Vector3(locations[transport.currentInterval].x, locations[transport.currentInterval].y, 0),
                new Vector3(locations[transport.currentInterval + 1].x, locations[transport.currentInterval + 1].y, 0),
                transport.progress
            );
        }
    }
}