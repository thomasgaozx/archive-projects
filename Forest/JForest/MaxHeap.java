package forest;

public class MaxHeap<T extends Comparable<T>> extends BinaryHeap<T>{

	public T max() throws NullPointerException {
		return peek();
	}

	public T min() throws NullPointerException {
		return getLeastImportantValue();
	} 

	public MaxHeap() {
		super(new ReverseComparator<T>());
	}
}