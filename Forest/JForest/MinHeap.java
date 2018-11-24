package forest;

public class MinHeap<T extends Comparable<T>> extends BinaryHeap<T>{

	public T max() throws NullPointerException {
		return getLeastImportantValue();

	}

	public T min() throws NullPointerException {
		return peek();
	}

	public MinHeap() {
		super(new NormalComparator<T>());
	}
}