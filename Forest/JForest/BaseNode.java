package forest;

class BaseNode<T> {
	private T value;

	public void setValue(T newValue) {
		value = newValue;
	}

	public T getValue() {
		return value;
	}

	public BaseNode(T val) {
		this.value = val;
	}
}