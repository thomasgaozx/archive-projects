@ECHO OFF

REM level 1 classes

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . Tree.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . Message.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . NormalComparator.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . ReverseComparator.java

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . util\SortFunction.java

ECHO level 1 build complete ...

REM dependency

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . BaseNode.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . BinaryNode.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . DoublyLinkedBinaryNode.java

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . util\MergeSort.java

ECHO node dependency build complete ...

REM major structures

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . BinaryHeap.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . array\ArrayHeap.java

ECHO major strutures build complete ...

REM deliverables

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . MinHeap.java
C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . MaxHeap.java

C:\Users\z72gao\Desktop\CURRENT\tools\bin\javac -Xlint -d . util\MergeSortFunction.java

ECHO deliverables build complete ...

ECHO build finished ...

PAUSE