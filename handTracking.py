import cv2
import mediapipe as mp
import numpy as np
import socket
import time

UDP_IP = "127.0.0.1"
BATTERYPORT = 5421
batterySock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
HANDPORT = 5060
handSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
LEDPORT = 7272
ledSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
WIREPORT = 7262
wireSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
RESISTORPORT = 7400
resistorSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)



class handTracker():
    def __init__(self, mode=False, maxHands=2, detectionCon=0.5,modelComplexity=1,trackCon=0.5):
        self.mode = mode
        self.maxHands = maxHands
        self.detectionCon = detectionCon
        self.modelComplex = modelComplexity
        self.trackCon = trackCon
        self.mpHands = mp.solutions.hands
        self.hands = self.mpHands.Hands(self.mode, self.maxHands,self.modelComplex,
                                        self.detectionCon, self.trackCon)
        self.mpDraw = mp.solutions.drawing_utils

    def handsFinder(self,image,draw=True):
        imageRGB = cv2.cvtColor(image,cv2.COLOR_BGR2RGB)
        self.results = self.hands.process(imageRGB)

        if self.results.multi_hand_landmarks:
            for handLms in self.results.multi_hand_landmarks:

                if draw:
                    self.mpDraw.draw_landmarks(image, handLms, self.mpHands.HAND_CONNECTIONS)
        return image

    def positionFinder(self,image, handNo=0, draw=True):
        lmlist = []

        if self.results.multi_hand_landmarks:
            Hand = self.results.multi_hand_landmarks[0]
            for id, lm in enumerate(Hand.landmark):

                h,w,c = image.shape
                cx,cy = int(lm.x*w), int(lm.y*h)
                lmlist.append([id,cx,cy])
            if draw:
                cv2.circle(image,(cx,cy), 15 , (255,0,255), cv2.FILLED)

        return lmlist

def distance(pos1, pos2):
    pos1 = np.array(pos1[1:])
    pos2 = np.array(pos2[1:])
    return np.sqrt(np.sum((pos1-pos2)**2))


def handSize(lmList):
    return distance(lmList[5], lmList[0])

def handDistPercent(lmList):
    # 200- 80 for acceptable range
    size = max(min(handSize(lmList), 150), 30) - 30
    # print(handSize(lmList))
    return size/(150-30)

def indexCLOSED(lmList):
    if distance(lmList[4],lmList[8]) < 0.3*handSize(lmList):
        return True
    return False

def getThumbPos(lmList):
    return ([lmList[4][1],lmList[4][2]])


def getCartesionalMeters(lmList, scalar):
    thumbX, thumbY = getThumbPos(lmList)
    thumbX /= 600
    thumbY /= 475
    return np.array([thumbX*scalar, thumbY*scalar, handDistPercent(lmList)*scalar])


def main():
    cap = cv2.VideoCapture(1)
    tracker = handTracker()
    prevCart = np.array([0,0,0])
    while True:
        success,image = cap.read()
        image = tracker.handsFinder(image)
        lmList = tracker.positionFinder(image, handNo = 8, draw = False)

        if len(lmList) != 0:
            currCart = getCartesionalMeters(lmList,1)
            diffCart = np.round_((currCart - prevCart), decimals = 3)
            prevCart = currCart

            # encodingString = f"{indexCLOSED(lmList)}, {diffCart[0]},{-1*diffCart[1]},{diffCart[2]},"
            encodingString = f"{indexCLOSED(lmList)}, {25*currCart[0]},{16-16*currCart[1]-2},{20-20*currCart[2]},"
            print(encodingString)
            # if indexCLOSED(lmList):
                # print("SENT")
            batterySock.sendto((encodingString).encode(), (UDP_IP, BATTERYPORT) )
            handSock.sendto((encodingString).encode(), (UDP_IP, HANDPORT) )
            ledSock.sendto((encodingString).encode(), (UDP_IP, LEDPORT) )
            wireSock.sendto((encodingString).encode(), (UDP_IP, WIREPORT) )
            resistorSock.sendto((encodingString).encode(), (UDP_IP, RESISTORPORT) )
        cv2.imshow("Video",image)
        cv2.waitKey(1)

if __name__ == "__main__":
    main()
