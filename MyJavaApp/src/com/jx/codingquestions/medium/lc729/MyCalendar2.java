package com.jx.codingquestions.medium.lc729;

import java.util.ArrayList;
import java.util.List;

public class MyCalendar2 implements MyCalendar {

    private final List<Integer> myStarts;
    private final List<Integer> myEnds;

    public MyCalendar2() {

        myStarts = new ArrayList<>();
        myEnds = new ArrayList<>();
    }

    @Override
    public boolean book(final int start, final int end) {

        if (!isValidEvent(start, end)) {
            return false;
        }

        final int indexToInsert = findIndexToInsert(start, end);
        if (indexToInsert == -1) {
            return false;
        }

        myStarts.add(indexToInsert, start);
        myEnds.add(indexToInsert, end);

        return true;
    }

    @Override
    public void printQueue() {

        for (int i = 0; i < myStarts.size(); i++) {
            System.out.printf("(%s, %s), ", myStarts.get(i), myEnds.get(i));
        }
        System.out.println();
    }

    private boolean isValidEvent(final int start, final int end) {

        return start < end && start >= 0;
    }

    private int findIndexToInsert(final int start, final int end) {

        for (int i = 0; i < myStarts.size(); i++) {
            final int curStart = myStarts.get(i);
            final int curEnd = myEnds.get(i);

            if (end <= curStart) {
                return i;
            }
            if (start < curEnd) {
                return -1;
            }
        }

        return myStarts.size();
    }
}
