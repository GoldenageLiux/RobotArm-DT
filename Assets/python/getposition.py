import cv2
import numpy as np
import transform
import json

def extract(img, n):  # 显示某颜色区域
    positions_color = []
    hsv = cv2.cvtColor(img, cv2.COLOR_RGB2HSV)

    # blue
    if n == "0":
        low = np.array([0, 43, 46])
        up = np.array([8, 255, 255])
    # green
    elif n == "1":
        low = np.array([45, 43, 46])
        up = np.array([70, 255, 255])
    # red
    elif n == "2":
        low = np.array([115, 43, 46])
        up = np.array([125, 255, 255])

    mask = cv2.inRange(hsv, low, up)
    # cv2.imshow("1", mask)
    ret, thresh1 = cv2.threshold(mask, 200, 255, cv2.THRESH_BINARY)
    guass = cv2.GaussianBlur(thresh1, (5, 5), 0)
    # cv2.imshow("2", guass)
    kernel = np.ones([5, 5], np.uint8)
    opening = cv2.morphologyEx(guass, cv2.MORPH_OPEN, kernel)
    ret, thresh1 = cv2.threshold(opening, 200, 255, cv2.THRESH_BINARY)
    dst = cv2.erode(thresh1, kernel, iterations=2)
    guass = cv2.GaussianBlur(dst, (9, 9), 0)
    ret, thresh1 = cv2.threshold(guass, 200, 255, cv2.THRESH_BINARY)
    dst = cv2.dilate(thresh1, (5, 5), iterations=1)
    # cv2.imshow("3", thresh1)
    ret, thresh1 = cv2.threshold(guass, 200, 255, cv2.THRESH_BINARY)
    # cv2.imshow("4", thresh1)
    contours, hierarchy = cv2.findContours(thresh1, 1, 2)
    # print('len(contours):', len(contours))
    for i in range(0, len(contours)):
        cnt_max = contours[i]
        x_max, y_max, w_max, h_max = cv2.boundingRect(cnt_max)
        center_max_x = x_max + w_max / 2
        center_max_y = y_max + h_max / 2
        max_x_, max_y_ = transform.transform_xy(center_max_x, center_max_y)

        if max_x_ > 80 and max_x_ < 310 and max_y_ < 90 and max_y_ > -85:
            positions_color.append([max_x_, max_y_])
    return positions_color

myCap = cv2.VideoCapture(0)
# myCap.set(cv2.CAP_PROP_FRAME_WIDTH, 1920)
# myCap.set(cv2.CAP_PROP_FRAME_HEIGHT, 1080)

# cap = []
# for i in range(3):
#     cap.append(cv2.VideoCapture(i))
#     PROP_BRIGHTNESS = cap[i].get(cv2.CAP_PROP_BRIGHTNESS)
#     if PROP_BRIGHTNESS==0:
#         myCap = cap[i]
        # break
myCap.set(cv2.CAP_PROP_FRAME_WIDTH, 1920)
myCap.set(cv2.CAP_PROP_FRAME_HEIGHT, 1080)

positions = {"blue": [], "green": [], "red": []}

if __name__ == '__main__':
    ret, frame = myCap.read()

    positions["blue"] = extract(frame, "0")
    positions["green"] = extract(frame, "1")
    positions["red"] = extract(frame, "2")

    positions_json = json.dumps(positions)  # 转化为json格式文件

    # 将json文件保存为.json格式文件
    with open('positions.json', 'w+') as file:
        file.write(positions_json)