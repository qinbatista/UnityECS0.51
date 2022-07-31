1: subsence is from Entity
2: subsence is binary data so it is very fast
3: subsence is used to create massive scene
4: Entities.ForEach.Run() is used for certain things which need to happened in main thread
5: Entities.ForEach.Schedule() will cause error if real time functions used like Time.deltaTime is used.
6: How many time left from previews frames
7: Entities.ForEach.Run() is on main thread
