package com.jx.codingquestions.medium.lc729;

import java.util.ArrayList;
import java.util.List;

public class MyCalendar1 implements MyCalendar {

    private final List<MyEvent> events;

    public MyCalendar1() {

        events = new ArrayList<>();
    }

    @Override
    public boolean book(final int start, final int end) {

        if (!isValidEvent(start, end)) {
            return false;
        }

        final MyEvent event = new MyEvent(start, end);
        final int i = findIndexToInsert(event);

        if (i == -1) {
            return false;
        }

        events.add(i, event);
        return true;
    }

    @Override
    public void printQueue() {
        for(final MyEvent event : events){
            System.out.printf("(%s, %s), ", event.start, event.end);
        }
        System.out.println();
    }

    private boolean isValidEvent(final int start, final int end) {

        return start < end && start >= 0;
    }

    private int findIndexToInsert(final MyEvent newEvent) {

        for (int i = 0; i < events.size(); i++) {
            final MyEvent curEvent = events.get(i);
            final MyStatus compareStatus = curEvent.compareEvents(newEvent);
            if (compareStatus.equals(MyStatus.OVERLAPPING)) {
                return -1;
            }
            if (compareStatus.equals(MyStatus.IN_FRONT)) {
                return i;
            }
        }

        return events.size();
    }

    static class MyEvent {

        private final int start;
        private final int end;

        private MyEvent() {

            start = -1;
            end = -1;
        }

        MyEvent(final int start, final int end) {

            this.start = start;
            this.end = end;
        }

        MyStatus compareEvents(final MyEvent newEvent) {

            if (newEvent.end <= start) {
                return MyStatus.IN_FRONT;
            } else if (newEvent.start >= end) {
                return MyStatus.BEHIND;
            } else {
                return MyStatus.OVERLAPPING;
            }
        }
    }

    enum MyStatus {

        // for event1.compareEvents(event2), event2 is in front of event1 without overlapping
        IN_FRONT,
        // for event1.compareEvents(event2), event2 is overlapping with even1
        OVERLAPPING,
        // for event1.compareEvents(event2), event2 is behind event1 without overlapping
        BEHIND
    }
}
