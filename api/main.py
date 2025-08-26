from fastapi import FastAPI, File, UploadFile
from fastapi.responses import JSONResponse
from ultralytics import YOLO
from PIL import Image
import io, time

app = FastAPI(title="Smart Part Detector", version="0.1.0")

MODEL_PATH = "runs/detect/spdet/weights/best.pt"
try:
    model = YOLO(MODEL_PATH)
except Exception:
    model = YOLO("yolov8n.pt")  # placeholder until you train
CLASS_NAMES = model.names

@app.get("/")
def root():
    return {"ok": True}

@app.post("/detect")
async def detect(file: UploadFile = File(...)):
    img = Image.open(io.BytesIO(await file.read())).convert("RGB")
    t0 = time.time()
    results = model.predict(img, imgsz=640, conf=0.25, verbose=False)
    ms = (time.time() - t0) * 1000
    det = []
    for r in results:
        for b in r.boxes:
            x1, y1, x2, y2 = [float(v) for v in b.xyxy[0].tolist()]
            cls_id = int(b.cls[0])
            conf = float(b.conf[0])
            det.append({
                "class": CLASS_NAMES.get(cls_id, str(cls_id)),
                "confidence": round(conf, 4),
                "bbox": [round(x1,2), round(y1,2), round(x2,2), round(y2,2)]
            })
    return JSONResponse({"detections": det, "inference_ms": round(ms,2)})
