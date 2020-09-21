const router = require('express').Router();
const controller = require('../controllers/controllers');

router.get('/', controller.index);
router.get('/accesspoint', controller.accesspoint);
router.get('/search', controller.search);

module.exports = router;