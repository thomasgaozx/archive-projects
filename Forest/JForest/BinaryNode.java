package forest;

class BinaryNode<T> extends BaseNode<T> {
	public BinaryNode<T> left;
	public BinaryNode<T> right;

	public BinaryNode(T val, BinaryNode<T> left, BinaryNode<T> right) {
		super(val);
		this.left = left;
		this.right = right;
	} 
}