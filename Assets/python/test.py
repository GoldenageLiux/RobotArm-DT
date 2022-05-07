import time

import cv2
import numpy as np
import transform
import sys

recognition_type = sys.argv[1]

def extract(img, n):  # 显示某颜色区域

    hsv = cv2.cvtColor(img, cv2.COLOR_RGB2HSV)

    # blue
    if n == "3":
        low = np.array([0, 43, 46])
        up = np.array([10, 255, 255])
    # green
    elif n == "4":
        low = np.array([52, 43, 46])
        up = np.array([62, 255, 255])
    # red
    elif n == "5":
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
    temp = [0, 0]
    for i in range(0, len(contours)):
        cnt = contours[i]
        # print('cnt:', cnt)
        area = cv2.contourArea(cnt)
        # print('area:', area)
        if area > temp[0]:
            temp[0] = area
            temp[1] = i
        # print('kkkkk')
    img3 = cv2.drawContours(img, contours, temp[1], (255, 0, 0), 3)
    cv2.imshow("end", img3)
    cnt = contours[temp[1]]
    x, y, w, h = cv2.boundingRect(cnt)
    center_x = x + w / 2
    center_y = y + h / 2

    # set y offset
    # center_y = 720 - center_y
    # print('center_x,center_y:', center_x, center_y)
    x_,y_ = transform.transform_xy(center_x,center_y)
    x_ = x_ - 5
    y_  = y_ + 5
    # print('center_x,center_y:', center_x, center_y)
    print( x_,',',y_)

    im = cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)
    # cv2.imshow("co" + str(n), im)
    # if center_x > 220 and center_x < 420:
    #     return 'ok'
    # else:
    #     return 'no'

cap = []
for i in range(3):
    cap.append(cv2.VideoCapture(i))
    PROP_BRIGHTNESS = cap[i].get(cv2.CAP_PROP_BRIGHTNESS)
    if PROP_BRIGHTNESS==0:
        myCap = cap[i]

myCap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
myCap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720)

# if __name__ == '__main__':
#     while(True):
#         ret, frame = myCap.read()
#
#         extract(frame, recognition_type)
#
#         if cv2.waitKey(10) & 0xFF == ord('q'):
#             break

ret, frame = myCap.read()

extract(frame, recognition_type)


