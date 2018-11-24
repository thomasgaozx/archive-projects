package forest;

import java.util.*;

/// by default a min heap!
public class BinaryHeap<T> {

	private DoublyLinkedBinaryNode<T> root;
	private Comparator<T> comparator;
	private int size;

	private static <E> void swapValue(DoublyLinkedBinaryNode<E> a, DoublyLinkedBinaryNode<E> b) {
		E temp  = a.getValue();
		a.setValue(b.getValue());
		b.setValue(temp);
	}

	/** returns a stack that indicates the directions to take to reach the 
	   desired node: true means turning left, false means turning right, 
	   backed by proof */
	private static Stack<Boolean> getDirectionsToPosition(int pos) {

		Stack<Boolean> directions = new Stack<Boolean>();

		while (pos>1) {
			int newVal = pos/2;
			if (newVal*2==pos) { // even, go left
				directions.push(new Boolean(true));
			} else { //odd, go right
				directions.push(new Boolean(false));
			}
			pos = newVal;
		}

		return directions;
	}

	private void percolateUp(DoublyLinkedBinaryNode<T> position) {

		// percolate Up
		boolean toClimb = true;
		
		while (toClimb && position.parent!=null) {
			int result = comparator.compare(position.getValue(),position.parent.getValue());

			if (result<0) {
				swapValue(position, position.parent);
			} else {
				toClimb = false;
			}
		}

	}

	private void percolateDown(DoublyLinkedBinaryNode<T> position) {
		boolean toContinue = true;

		while (toContinue) {
			// position can only have none, left or both left and right

			T currentValue = position.getValue();
			T leftValue = position.left==null ? null : position.left.getValue();
			T rightValue = position.right==null ? null : position.right.getValue();

			if (leftValue==null) { //have none
				toContinue=false;

			} else if (rightValue==null) { //have left ONLY, and is the last swap no matter what
				if (comparator.compare(currentValue, leftValue)>0) {
					swapValue(position,position.left);
				}
				toContinue=false;

			} else { // both branches exist,
				int resultLeftVsRight = comparator.compare(leftValue,rightValue);

				if (resultLeftVsRight>0) { // but right is smaller
					if (comparator.compare(currentValue, rightValue)>0) {
						swapValue(position, position.right);
						position=position.right;
					} else {
						toContinue=false;
					}

				} else { // but left is smaller or equal
					if (comparator.compare(currentValue,leftValue)>0) {
						swapValue(position,position.left);
						position=position.left;
					} else {
						toContinue=false;
					}
				}
			}
		}
	}

	// peek the top node
	protected T peek() throws NullPointerException {
		if (root==null) {
			throw new NullPointerException(Message.emptyTree);
		} else {
			return root.getValue();
		}
	}

	public boolean isEmpty() {
		return root==null;
	}

	public void clear() {
		root=null;
	}

	public T pop() throws NullPointerException {
		if (root==null) {
			throw new NullPointerException(Message.emptyTree);
		}

		T temp = root.getValue();

		Stack<Boolean> directions = getDirectionsToPosition(size);

		if (directions.empty()) { // if root is the only entry
			root=null;
			return temp;
		}

		// true means turning left
		DoublyLinkedBinaryNode<T> position = root;
		while (!directions.empty()) {
			if (directions.pop().booleanValue()) { // true, turn left
				position=position.left; // boldly go ahead because position will not be null
			} else { // turn right
				position=position.right;
			}
		}

		// now position is at the right place
		root.setValue(position.getValue());
		if (position.parent.left!=null && comparator.compare(position.parent.left.getValue(), position.getValue())==0) { 
			// parent's left link is position
			position.parent.left=null;
		} else {
			position.parent.right=null;
		}
		--size;

		// percolate down from root
		percolateDown(root);

		return temp;
	}

	// for min heap, this is going to return the max value, use depth first search just to show off
	protected T getLeastImportantValue() throws NullPointerException {
		if (root==null) {
			throw new NullPointerException(Message.emptyTree);
		}

		T leastImportantValue = root.getValue();

		Stack<DoublyLinkedBinaryNode<T>> nodes = new Stack<DoublyLinkedBinaryNode<T>>();
		nodes.push(root);
		while (!nodes.empty()) {
			DoublyLinkedBinaryNode<T> position = nodes.pop();

			if (position.left!=null) {
				if (position.right!=null) {
					nodes.push(position.right);
				}
				nodes.push(position.left);
			} else if (comparator.compare(position.getValue(),leastImportantValue)>0) { 
				/* firstly, left is null, right must be null too.
				 * then, since it's the largest possible value of the branch, 
				 * compare it with the stored least important value
				 */
				leastImportantValue=position.getValue();
			}

		}

		return leastImportantValue;
	}

	// used breadth first search for efficiency
	public boolean contains(T obj) {
		if (root==null) {
			return false;
		}

		Queue<DoublyLinkedBinaryNode<T>> nodesToCheck= new LinkedList<DoublyLinkedBinaryNode<T>>();
		nodesToCheck.add(root);

		while (!nodesToCheck.isEmpty()) {
			DoublyLinkedBinaryNode<T> position = nodesToCheck.remove();

			int result = comparator.compare(obj, position.getValue());
			
			if (result==0) {
				return true;
			} else if (result>0) {
				if (position.left!=null) {
					nodesToCheck.add(position.left);
					if (position.right!=null) {
						nodesToCheck.add(position.right);
					}
				}
			}
		}

		return false;
	}

	public boolean insert(T obj) {
		if (root==null) {
			root = new DoublyLinkedBinaryNode<T>(obj,null,null,null);
			++size;
			return true;
		} 

		// follow the directions, true means turning left and false meaning turning right
		Stack<Boolean> directions = getDirectionsToPosition(size+1);

		DoublyLinkedBinaryNode<T> position=root;
		while (!directions.empty()) {
			if (directions.pop().booleanValue()) { // even, thus go left
				if (directions.empty()) {
					position.left = new DoublyLinkedBinaryNode<T>(obj,null,null,position);
				} 
				position = position.left;

			} else { // odd, thus go right
				if (directions.empty()) {
					position.right = new DoublyLinkedBinaryNode<T>(obj,null,null, position);
				} 
				position=position.right;	
			}
		}

		// position is now on the newly inserted node
		percolateUp(position);

		++size;
		return true;
	}

	public BinaryHeap(Comparator<T> comparator) {
		root=null;
		this.comparator=comparator;
		this.size=0;
	}

}