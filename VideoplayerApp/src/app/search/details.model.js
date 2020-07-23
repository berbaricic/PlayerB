"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var VideoDetail = /** @class */ (function () {
    function VideoDetail(obj) {
        this.id = obj && obj.id || null;
        this.title = obj && obj.title || null;
        this.description = obj && obj.description || null;
        this.thumbnailUrl = obj && obj.thumbnailUrl || null;
        this.videoUrl = obj && obj.videoUrl || "https://www.youtube.com/watch?v=" + this.id;
    }
    return VideoDetail;
}());
exports.VideoDetail = VideoDetail;
//# sourceMappingURL=details.js.map