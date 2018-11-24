package forest;

class DoublyLinkedBinaryNode<T> extends BaseNode<T> {
	
	public DoublyLinkedBinaryNode<T> left;
	public DoublyLinkedBinaryNode<T> right;
	public DoublyLinkedBinaryNode<T> parent;

	public DoublyLinkedBinaryNode(T val, DoublyLinkedBinaryNode<T> left, DoublyLinkedBinaryNode<T> right, DoublyLinkedBinaryNode<T> parent) {
		super(val);
		this.left=left;
		this.right=right;
		this.parent = parent;
	}
}