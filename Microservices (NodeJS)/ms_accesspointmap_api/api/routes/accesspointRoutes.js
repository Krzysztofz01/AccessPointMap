const router = require('express').Router();
const controller = require('../controller/accesspointController.js');
const { authWrite, authRead } = require('../auth/accesspointAuth.js');

//All routes from /projects/accesspointmap/api/request
router.post('/add', authWrite, controller.add);
router.get('/read', authRead, controller.readAll);
router.get('/read/id/:id', authRead, controller.readById);
//Need to check !
router.get('/search', authRead, controller.search);

module.exports = router;