# Smart Part Detector (YOLOv8 + FastAPI)
Detect bolts/nuts/parts. Stack: Ultralytics YOLOv8, FastAPI, ONNX, Docker.

## Roadmap
- [ ] Collect & label 250+ images
- [ ] Train YOLOv8n
- [ ] Export ONNX (optional)
- [ ] FastAPI `/detect`
- [ ] Android demo
- [ ] Docker + CI

## Data layout
data/
  dataset.yaml
  images/train, val
  labels/train, val
