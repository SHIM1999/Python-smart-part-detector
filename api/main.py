from fastapi import FastAPI, File, UploadFile, HTTPException
from fastapi.responses import JSONResponse
from fastapi.middleware.cors import CORSMiddleware
from ultralytics import YOLO
from PIL import Image, UnidentifiedImageError
import io, time

APP_TITLE = "Smart Part Detector"
APP_VERSION = "0.1.1"

app = FastAPI(
    title=APP_TITLE,
    version=APP_VERSION,
    description="YOLOv8 + FastAPI demo for industrial part detection",
)

# --- CORS (dev-friendly: allow all; tighten later) ---
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # change to your domains for prod
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# --- Model loading ---
MODEL_PATH = "runs/detect/spdet/weights/best.pt"  # replace after training
_loaded_from = "yolov8n.pt (placeholder)"
try:
    model = YOLO(MODEL_PATH)
    _loaded_from = MODEL_PATH
except Exception:
    # Fallback to tiny pretrained model so API still works before training
    model = YOLO("yolov8n.pt")

CLASS_NAMES = model.names  # dict {id: name}

@app.get("/")
def root():
    """Health/info endpoint."""
    return {
        "ok": True,
        "app": APP_TITLE,
        "version": APP_VERSION,
        "model_loaded_from": _loaded_from,
        "num_classes": len(CLASS_NAMES),
        "docs": "/docs",
    }

@app.get("/labels")
def labels():
    """Return class id -> name mapping."""
    return CLASS_NAMES

@app.post("/detect")
async def detect(file: UploadFile = File(...)):
    # Basic validations
    if not file.content_type or not file.content_type.startswith(("image/",)):
        raise HTTPException(status_code=415, detail="Unsupported media type. Upload an image file.")
    try:
        content = await file.read()
        image = Image.open(io.BytesIO(content)).convert("RGB")
    except UnidentifiedImageError:
        raise HTTPException(status_code=400, detail="Invalid image data.")

    t0 = time.time()
    results = model.predict(image, imgsz=640, conf=0.25, verbose=False)
    ms = (time.time() - t0) * 1000.0

    detections = []
    for r in results:
        for b in r.boxes:
            x1, y1, x2, y2 = [float(v) for v in b.xyxy[0].tolist()]
            cls_id = int(b.cls[0])
            conf = float(b.conf[0])
            detections.append({
                "class": CLASS_NAMES.get(cls_id, str(cls_id)),
                "confidence": round(conf, 4),
                "bbox": [round(x1, 2), round(y1, 2), round(x2, 2), round(y2, 2)],
            })

    return JSONResponse({
        "detections": detections,
        "count": len(detections),
        "inference_ms": round(ms, 2),
        "model": _loaded_from,
    })
