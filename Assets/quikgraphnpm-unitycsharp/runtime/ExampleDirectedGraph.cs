using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuikGraph;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Observers;

public class ExampleDirectedGraph : MonoBehaviour
{
    public float nodeRenderSizeX;
    public float nodeRenderSizeY;
    public float nodeRenderSizeZ;
    private Vertex va = new Vertex(
        new Vector2(0, 0)
    );
    private Vertex vb = new Vertex(
        new Vector2(1, 1)
    );
    private Vertex vc = new Vertex(
        new Vector2(1, -1)
    );
    private Vertex vd = new Vertex(
        new Vector2(2, 0)
    );

    private Vertex[] _vertices;
    private SEdge<Vertex>[] _edges;
    private Vector3 _nodeRenderSize;
    private BidirectionalGraph<Vertex, SEdge<Vertex>>
        _graph;

    // Start is called before the first frame update
    void Start()
    {
        _nodeRenderSize = new Vector3(
            nodeRenderSizeX,
            nodeRenderSizeY,
            nodeRenderSizeZ
        );

        _vertices = new Vertex[] {
            va,
            vb,
            vc,
            vd
        };
        
        _edges = new SEdge<Vertex>[] {
            new SEdge<Vertex>(va, vb),
            new SEdge<Vertex>(va, vc),
            new SEdge<Vertex>(vb, vd),
            new SEdge<Vertex>(vc, vd)
        };

        _graph = BuildGraph(_edges);
        
        VertexPredecessorRecorderObserver<Vertex, SEdge<Vertex>> observer =
            new VertexPredecessorRecorderObserver<Vertex, SEdge<Vertex>>();

        SearchGraph(_graph, observer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public struct Vertex {
        public Vertex(Vector2 point) {
            Point = point;
        }

        public Vector2 Point { get; }
    }

    void OnDrawGizmos() {
        RenderGraph();
    }

    BidirectionalGraph<Vertex, SEdge<Vertex>> BuildGraph(
        SEdge<Vertex>[] edges
    ) {
        BidirectionalGraph<Vertex, SEdge<Vertex>> result =
            _edges.ToBidirectionalGraph<Vertex, SEdge<Vertex>>();
        return result;
    }

    void SearchGraph(
        BidirectionalGraph<Vertex, SEdge<Vertex>> graph,
        VertexPredecessorRecorderObserver<Vertex, SEdge<Vertex>>
            observer = null
    ) {
        BreadthFirstSearchAlgorithm<Vertex, SEdge<Vertex>> bfs =
            new BreadthFirstSearchAlgorithm<Vertex, SEdge<Vertex>>
                (graph);

        if (observer == null) {
            bfs.Compute();
        }
        else {
            using (observer.Attach(bfs)) {
            bfs.Compute();

            foreach (
                KeyValuePair<Vertex, SEdge<Vertex>> pair in
                observer.VerticesPredecessors
            ) {
                Debug.Log(
                    pair.Value.Source.Point +
                    " -> " +
                    pair.Value.Target.Point
                );
            }
        }   
        }
    }

    void RenderGraph() {
        if (_edges == null || _vertices == null)
            return;

        foreach (SEdge<Vertex> edge in _edges) {
            Debug.DrawLine(
                edge.Source.Point,
                edge.Target.Point,
                Color.green,
                Time.deltaTime
            );
        }

        foreach (Vertex v in _vertices) {
            Gizmos.DrawCube(
                v.Point,
                _nodeRenderSize
            );
        }
    }

    void LogGraph() {

    }
}
