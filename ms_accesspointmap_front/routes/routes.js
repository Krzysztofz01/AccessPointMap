const router = require('express').Router();
const controller = require('../controllers/controller.js');

router.get('/', controller.index);
router.get('/accesspoint/id/:id', controller.accesspoint);
router.get('/search', controller.search);

module.exports = router;