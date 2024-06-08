using System.Collections.Generic;

namespace GamePlay.General
{
    public interface IGraph<T>
    {
        public IList<IGraphNode<T>> nodes { get; set; }

        public IList<IGraphEdge<T>> edges { get; set; }

        public void AddNode(IGraphNode<T> node);

        public void AddEdge(IGraphNode<T> node1, IGraphNode<T> node2);
    }

    public interface IGraphNode<T>
    {
        public T value {
            get;
            set;
        }
    }
    public interface IGraphEdge<T>
    {
        public IGraphNode<T> node1 { get; set; }
        public IGraphNode<T> node2 { get; set; }
    }
}
