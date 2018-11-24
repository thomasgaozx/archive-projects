package forest.util;

import java.util.*;

// by default sort in ascending order
public interface SortFunction {

	public <T> void sort(List<T> list, Comparator<T> comparator);

}