import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class Main {

    public static void main(String[] args) {

        System.out.println("Hello World!");

        Runnable task1 = generateRunnable("TaskA", 2);
        Runnable task2 = generateRunnable("TaskB", 1);
        Runnable task3 = generateRunnable("LongTask", 20);

        ExecutorService executor = Executors.newCachedThreadPool();
        executeRunnable(executor, task1, task2, task3);

        shutdownEverything(executor);
    }

    private static Runnable generateRunnable(String taskName) {

        return generateRunnable(taskName, 1);
    }

    private static Runnable generateRunnable(String taskName, int secondToSleep) {

        return () -> {
            try {
                final String threadName = Thread.currentThread().getName();
                System.out.println(String.format("thread [%s] starting", threadName));

                TimeUnit.SECONDS.sleep(secondToSleep);

                System.out.println(String.format("thread [%s] ending", threadName));
                System.out.println(String.format("Task [%s]_[%s} succeed!", taskName, threadName));
            } catch (InterruptedException e) {
                System.out.println(String.format("Task [%s] getting interrupted", taskName));
                e.printStackTrace();
                Thread.currentThread().interrupt();
            }
        };
    }

    private static void executeRunnable(ExecutorService executor, Runnable... runnables) {

        for (int i = 0; i < 3; i++) {
            for (Runnable runnable : runnables) {
                executor.submit(runnable);
            }
        }
    }

    private static void shutdownEverything(ExecutorService executor) {

        boolean success = false;

        try {
            executor.shutdown();

            if (!executor.awaitTermination(10, TimeUnit.SECONDS)) {
                executor.shutdownNow();
            } else {
                success = true;
            }
        } catch (InterruptedException e) {
            executor.shutdownNow();
            Thread.currentThread().interrupt();
            System.out.println("catching again... ");
        }
    }
}
