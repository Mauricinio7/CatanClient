using CatanClient.GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Gameplay.Helpers
{
    public static class GameRules
    {
        public static bool IsRoadConnectedToPlayerSettlementOrRoad(GameBoardStateDto gameBoardState, int edgeId, int playerId)
        {
            var edge = gameBoardState.HexTiles
                .SelectMany(h => h.Edges)
                .FirstOrDefault(e => e.Id == edgeId);

            if (edge == null)
                return false;

            // Verificar conexión con asentamientos del jugador
            var isConnectedToSettlement = edge.ConnectedHexes
                .SelectMany(hId => gameBoardState.HexTiles.First(h => h.Id == hId).Vertices)
                .Any(v => v.IsOccupied && v.OwnerPlayerId == playerId);

            // Verificar conexión con carreteras del jugador
            var isConnectedToRoad = edge.ConnectedEdges
                .SelectMany(eId => gameBoardState.HexTiles
                    .SelectMany(h => h.Edges)
                    .Where(e => e.Id == eId))
                .Any(e => e.IsOccupied && e.OwnerPlayerId == playerId);

            return isConnectedToSettlement || isConnectedToRoad;
        }

        public static bool IsVertexAvailableForSettlement(GameBoardStateDto gameBoardState, int vertexId, int playerId)
        {
            var vertex = gameBoardState.HexTiles
                .SelectMany(h => h.Vertices)
                .FirstOrDefault(v => v.Id == vertexId);

            if (vertex == null || vertex.IsOccupied)
                return false;

            // Obtener los vértices adyacentes (usando las aristas conectadas)
            IEnumerable<VertexDto> adjacentVertices = vertex.ConnectedEdges
                .SelectMany(eId => gameBoardState.HexTiles
                    .SelectMany(h => h.Edges)
                    .First(e => e.Id == eId)
                    .ConnectedHexes
                    .SelectMany(hId => gameBoardState.HexTiles.First(h => h.Id == hId).Vertices))
                .Where(v => v.Id != vertexId) // Excluir el vértice actual
                .Distinct();

            // Verificar que ningún vértice adyacente esté ocupado
            if (adjacentVertices.Any(v => v.IsOccupied))
                return false;

            return true;
        }
    }
}
