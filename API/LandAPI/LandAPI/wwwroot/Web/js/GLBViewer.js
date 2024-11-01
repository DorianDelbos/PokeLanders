class GLBViewer extends HTMLElement {
    constructor() {
        super();

        // Attach a shadow DOM to the element
        this.attachShadow({ mode: 'open' });

        // Initialize the WebGL renderer
        this.renderer = new THREE.WebGLRenderer({ alpha: true });
        this.shadowRoot.appendChild(this.renderer.domElement);

        // Create a scene and a camera
        this.scene = new THREE.Scene();
        this.camera = new THREE.PerspectiveCamera(
            75, // Field of view
            1, // Aspect ratio
            0.1, // Near clipping plane
            1000 // Far clipping plane
        );

        // Initialize properties
        this.model = null; // Store the loaded model
        this.autoRotate = false; // Auto-rotation flag
        this.controls = null; // Store OrbitControls if enabled

        // Add lighting to the scene
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.5); // Soft ambient light
        this.scene.add(ambientLight);
        const directionalLight = new THREE.DirectionalLight(0xffffff, 1); // Directional light
        directionalLight.position.set(0, 1, 1).normalize(); // Light position
        this.scene.add(directionalLight);

        // Bind the animate method to the current instance
        this.animate = this.animate.bind(this);
    }

    // Called when the element is added to the DOM
    connectedCallback() {
        // Set initial size of the renderer
        this.resizeRenderer();
        window.addEventListener('resize', this.resizeRenderer.bind(this));

        // Load the model from the 'src' attribute
        this.loadModel(this.getAttribute('src'));

        // Check for auto-rotation attribute
        this.autoRotate = this.hasAttribute('auto-rotate');

        // Set the camera position based on attributes
        this.setCameraPosition();

        // Initialize controls if 'camera-control' attribute is present
        if (this.hasAttribute('camera-control')) {
            this.controls = new THREE.OrbitControls(this.camera, this.renderer.domElement);
        }

        // Start the animation loop
        this.animate();
    }

    // Set the camera position based on attributes
    setCameraPosition() {
        const x = parseFloat(this.getAttribute('camera-x')) || 0; // X position
        const y = parseFloat(this.getAttribute('camera-y')) || 0; // Y position
        const z = parseFloat(this.getAttribute('camera-z')) || 5; // Z position

        const offsetx = parseFloat(this.getAttribute('offset-x')) || 0; // X position
        const offsety = parseFloat(this.getAttribute('offset-y')) || 0; // Y position
        const offsetz = parseFloat(this.getAttribute('offset-z')) || 0; // Z position

        this.camera.position.set(x, y, z); // Set the camera position
        this.camera.lookAt(offsetx, offsety, offsetz); // Look at the center of the scene
    }

    // Load the 3D model using the GLTFLoader
    loadModel(src) {
        const loader = new THREE.GLTFLoader();
        loader.load(src, (gltf) => {
            this.model = gltf.scene; // Store the loaded model
            this.scene.add(this.model); // Add the model to the scene
        }, undefined, (error) => {
            console.error('An error occurred while loading the model:', error); // Error handling
        });
    }

    // Function to parse dimensions with units
    parseDimension(attrValue, defaultValue) {
        if (!attrValue) {
            return defaultValue; // Return default if no attribute is set
        }

        const value = parseFloat(attrValue); // Get numeric value
        const unit = attrValue.replace(value, '').trim(); // Extract unit

        // Handle the case for percentage or other units
        if (unit === '%' || !unit) {
            // Calculate based on parent element size for percentage
            const parent = this.parentElement;
            if (parent) {
                return (value / 100) * parent.clientWidth; // Assume width for percentage
            } else {
                console.warn('No parent element found. Defaulting to width of the window.');
                return (value / 100) * window.innerWidth; // Fallback to window size
            }
        } else if (unit === 'px' || unit === 'em') {
            // Return pixel value directly
            return value;
        } else {
            console.warn(`Unsupported unit "${unit}", defaulting to pixels.`);
            return value; // Default to pixels if unit is unsupported
        }
    }

    // Resize the renderer and update camera aspect ratio
    resizeRenderer() {
        const widthAttr = this.getAttribute('width');
        const heightAttr = this.getAttribute('height');

        const width = this.parseDimension(widthAttr, this.parentElement.clientWidth || window.innerWidth); // Get width
        const height = this.parseDimension(heightAttr, this.parentElement.clientHeight || window.innerHeight); // Get height

        // Update the renderer size
        this.renderer.setSize(width, height);
        // Update the camera aspect ratio and projection matrix
        this.camera.aspect = width / height;
        this.camera.updateProjectionMatrix();
    }

    // Animation loop
    animate() {
        requestAnimationFrame(this.animate); // Request the next frame

        if (this.model) {
            // Rotate the model if auto-rotation is enabled
            if (this.autoRotate) {
                this.model.rotation.y += 0.01; // Rotate around the Y axis
            }
        }

        // Update controls if they are enabled
        if (this.controls) {
            this.controls.update();
        }

        // Render the scene from the camera's perspective
        this.renderer.render(this.scene, this.camera);
    }
}

// Define the custom element
customElements.define('glb-viewer', GLBViewer);
