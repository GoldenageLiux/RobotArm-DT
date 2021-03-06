import cv2
import numpy as np

def get_m(
          origin_points_set,
          target_points_set):
    # 确保两个点集的数量级不要差距过大，否则会输出None,看到这个输出，我直接好家伙。
    # 明明回家前，还能输出一个好的转换矩阵，为什么一回家就报错？我错哪儿了...
    m, i = cv2.estimateAffine2D(origin_points_set, target_points_set)
    return m

# camera = np.array( [(911.287, 579.779), (739.027,553.695), (561.97,526.253), (933.09,432.564), (761.24,407.534), (584.863,380.085), (949.163,286.763), (777.498,262.797), (601.398,235.389)] ).reshape(1,-1,2)
# robot = np.array( [(274.00 , -72.00), (210.60 ,-72.00), (143.60 ,-67.00), (275.00 , -15.00), (208.00 ,-12.00), (140.00 , -12.00), (274.00 , 41.00), (208.00 , 41.00),(145.00 , 43.00)] ).reshape(1,-1,2)

robot = np.array([(271.8, 46.3), (206.8, 46.3), (136.8, 46.3),
                  (266.8, -8.7), (206.8, -8.7), (136.8, -8.7),
                  (264.8, -63.7), (204.8, -63.7), (134.8, -63.7)])

camera = np.array([(1465.88, 386.08), (1203.09, 395.61), (935.14, 402.47),
                   (1480.24, 607.79), (1216.94, 615.16), (948.44, 619.96),
                   (1485.80, 832.24), (1222.13, 837.72), (953.22, 840.51)])

def transform_xy(camera_x, camera_y):
    m = get_m(camera, robot)
    print(m)
    A=m[0][0]
    B=m[0][1]
    C=m[0][2]
    D=m[1][0]
    E=m[1][1]
    F=m[1][2]
    robot_x = (A*camera_x)+(B*camera_y)+C
    robot_y = (D*camera_x)+(E*camera_y)+F
    return robot_x, robot_y
