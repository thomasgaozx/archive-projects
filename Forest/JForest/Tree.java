package forest;

import java.util.*;

public interface Tree<T extends Comparable<T>> {

	public T max() throws NullPointerException;

	public T min() throws NullPointerException;

	public boolean contains(T obj);

	public boolean insert(T obj);

	public boolean remove(T obj);

	public void clear();

	public boolean isEmpty();

}